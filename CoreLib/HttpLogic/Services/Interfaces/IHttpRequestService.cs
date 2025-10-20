using CoreLib.HttpLogic.Services;
using CoreLib.HttpLogic.Services;

namespace CoreLib.HttpLogic.Services.Interfaces;

/// <summary>
/// Отправка HTTP запросов и обработка ответов
/// </summary>
public interface IHttpRequestService
{
    /// <summary>
    /// Отправить HTTP-запрос
    /// </summary>
    Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData, HttpConnectionData connectionData = default);
}