using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Web;
using CoreLib.HttpLogic.Services.Interfaces;
using CoreLib.HttpLogic.Services.Interfaces;
using CoreLib.TraceLogic.Interfaces;
using CoreLib.HttpLogic.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreLib.HttpLogic.Services;

public enum ContentType
{
    ///
    Unknown = 0,

    ///
    ApplicationJson = 1,

    ///
    XWwwFormUrlEncoded = 2,

    ///
    Binary = 3,

    ///
    ApplicationXml = 4,

    ///
    MultipartFormData = 5,

    /// 
    TextXml = 6,

    /// 
    TextPlain = 7,

    ///
    ApplicationJwt = 8
}

public record HttpRequestData
{
    /// <summary>
    /// Тип метода
    /// </summary>
    public HttpMethod Method { get; set; }

    /// <summary>
    /// Адрес запроса
    /// </summary>\
    public Uri Uri { set; get; }

    /// <summary>
    /// Тело метода
    /// </summary>
    public object Body { get; set; }

    /// <summary>
    /// content-type, указываемый при запросе
    /// </summary>
    public ContentType ContentType { get; set; } = ContentType.ApplicationJson;

    /// <summary>
    /// Заголовки, передаваемые в запросе
    /// </summary>
    public IDictionary<string, string> HeaderDictionary { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Коллекция параметров запроса
    /// </summary>
    public ICollection<KeyValuePair<string, string>> QueryParameterList { get; set; } =
        new List<KeyValuePair<string, string>>();
}

public record BaseHttpResponse
{
    /// <summary>
    /// Статус ответа
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Заголовки, передаваемые в ответе
    /// </summary>
    public HttpResponseHeaders Headers { get; set; }

    /// <summary>
    /// Заголовки контента
    /// </summary>
    public HttpContentHeaders ContentHeaders { get; init; }

    /// <summary>
    /// Является ли статус код успешным
    /// </summary>
    public bool IsSuccessStatusCode
    {
        get
        {
            var statusCode = (int)StatusCode;

            return statusCode >= 200 && statusCode <= 299;
        }
    }
}

public record HttpResponse<TResponse> : BaseHttpResponse
{
    /// <summary>
    /// Тело ответа
    /// </summary>
    public TResponse Body { get; set; }
}

/// <inheritdoc />
internal class HttpRequestService : IHttpRequestService
{
    private readonly IHttpConnectionService _httpConnectionService;
    private readonly IEnumerable<ITraceWriter> _traceWriterList;

    ///
    public HttpRequestService(
        IHttpConnectionService httpConnectionService,
        IEnumerable<ITraceWriter> traceWriterList)
    {
        _httpConnectionService = httpConnectionService;
        _traceWriterList = traceWriterList;
    }

    /// <inheritdoc />
    public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData,
        HttpConnectionData connectionData)
    {
        var client = _httpConnectionService.CreateHttpClient(connectionData);

        var httpRequestMessage = CreateHttpRequestMessage(requestData);

        foreach (var traceWriter in _traceWriterList)
        {
            httpRequestMessage.Headers.Add(traceWriter.Name, traceWriter.GetValue());
        }

        var responseMessage = await _httpConnectionService.SendRequestAsync(
            httpRequestMessage, 
            client, 
            connectionData.CancellationToken);

        return await CreateHttpResponseAsync<TResponse>(responseMessage);
    }

    private static HttpContent PrepareContent(object body, ContentType contentType)
    {
        switch (contentType)
        {
            case ContentType.ApplicationJson:
                {
                    if (body is string stringBody)
                    {
                        body = JToken.Parse(stringBody);
                    }

                    var serializeSettings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    var serializedBody = JsonConvert.SerializeObject(body, serializeSettings);
                    var content = new StringContent(serializedBody, Encoding.UTF8, MediaTypeNames.Application.Json);
                    return content;
                }

            case ContentType.XWwwFormUrlEncoded:
                {
                    if (body is not IEnumerable<KeyValuePair<string, string>> list)
                    {
                        throw new Exception(
                            $"Body for content type {contentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");
                    }

                    return new FormUrlEncodedContent(list);
                }
            case ContentType.ApplicationXml:
                {
                    if (body is not string s)
                    {
                        throw new Exception($"Body for content type {contentType} must be XML string");
                    }

                    return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
                }
            case ContentType.Binary:
                {
                    if (body.GetType() != typeof(byte[]))
                    {
                        throw new Exception($"Body for content type {contentType} must be {typeof(byte[]).Name}");
                    }

                    return new ByteArrayContent((byte[])body);
                }
            case ContentType.TextXml:
                {
                    if (body is not string s)
                    {
                        throw new Exception($"Body for content type {contentType} must be XML string");
                    }

                    return new StringContent(s, Encoding.UTF8, MediaTypeNames.Text.Xml);
                }
            default:
                throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
        }
    }

    private HttpRequestMessage CreateHttpRequestMessage(HttpRequestData requestData)
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = requestData.Method,
            RequestUri = requestData.Uri
        };

        if (requestData.QueryParameterList.Any())
        {
            var uriBuilder = new UriBuilder(requestData.Uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var parameter in requestData.QueryParameterList)
            {
                query[parameter.Key] = parameter.Value;
            }

            uriBuilder.Query = query.ToString();
            requestMessage.RequestUri = uriBuilder.Uri;
        }

        if (requestData.Body != null)
        {
            requestMessage.Content = PrepareContent(requestData.Body, requestData.ContentType);
        }

        foreach (var header in requestData.HeaderDictionary)
        {
            requestMessage.Headers.Add(header.Key, header.Value);
        }
        
        return requestMessage;
    }

    private async Task<HttpResponse<TResponse>> CreateHttpResponseAsync<TResponse>(HttpResponseMessage responseMessage)
    {
        TResponse body = default;
        if (responseMessage.Content != null && typeof(TResponse) != typeof(object))
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            body = DeserializeContent<TResponse>(content, responseMessage.Content.Headers);
        }

        return new HttpResponse<TResponse>()
        {
            StatusCode = responseMessage.StatusCode,
            Headers = responseMessage.Headers,
            ContentHeaders = responseMessage.Content?.Headers,
            Body = body
        };
    }

    private TResponse? DeserializeContent<TResponse>(string content, HttpContentHeaders headers)
    {
        if (string.IsNullOrEmpty(content))
        {
            return default;
        }

        var contentType = headers.ContentType?.MediaType;

        if (contentType?.Contains("application/json") == true)
        {
            return JsonConvert.DeserializeObject<TResponse>(content);
        }
        
        throw new InvalidOperationException($"Unsupported content type: {contentType}");
    }
}