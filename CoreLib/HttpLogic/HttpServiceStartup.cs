using CoreLib.HttpLogic.Services.Interfaces;
using CoreLib.HttpLogic.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CoreLib.HttpLogic;

/// <summary>
/// ����������� � DI �������� ��� HTTP-����������
/// </summary>
public static class HttpServiceStartup
{
	/// <summary>
	/// ���������� ������� ��� ������������� �������� �� HTTP
	/// </summary>
	public static IServiceCollection AddHttpRequestService(this IServiceCollection services)
	{
		services
			.AddHttpContextAccessor()
			.AddHttpClient()
			.AddTransient<IHttpConnectionService, HttpConnectionService>();

		services.TryAddTransient<IHttpRequestService, HttpRequestService>();

		return services;
	}
}