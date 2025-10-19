using Application.Interfaces;
using CoreLib.HttpLogic.Services;
using CoreLib.HttpLogic.Services.Interfaces;

namespace Infrastructure.HttpClient;

public class UserApiClient : IUserApiClient
{
    private readonly IHttpRequestService _httpRequestService;
    private readonly string _userApiBaseUrl;

    public UserApiClient(IHttpRequestService httpRequestService, IConfiguration configuration)
    {
        _httpRequestService = httpRequestService;
        _userApiBaseUrl = configuration["UserApi:BaseUrl"] ?? throw new ArgumentNullException("UserApi:BaseUrl is not configured");
    }
    
    public async Task<bool> UserExistsAsync(Guid userId)
    {
        try
        {
            var requestData = new HttpRequestData
            {
                Method = HttpMethod.Get,
                Uri = new Uri($"{_userApiBaseUrl}/wishly/users/{userId}"),
                ContentType = ContentType.ApplicationJson
            };

            var connectionData = new HttpConnectionData
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            var response = await _httpRequestService.SendRequestAsync<object>(requestData, connectionData);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<UserInfoDto?> GetUserInfoAsync(Guid userId)
    {
        try
        {
            var requestData = new HttpRequestData
            {
                Method = HttpMethod.Get,
                Uri = new Uri($"{_userApiBaseUrl}/wishly/users/{userId}"),
                ContentType = ContentType.ApplicationJson
            };

            var connectionData = new HttpConnectionData
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            var response = await _httpRequestService.SendRequestAsync<UserInfoDto>(requestData, connectionData);
            if (response.IsSuccessStatusCode)
            {
                return response.Body;
            }

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}