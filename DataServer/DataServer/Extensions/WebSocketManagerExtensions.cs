using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Rubius.DevSaunaB.DataServer.Handlers;
using Rubius.DevSaunaB.DataServer.Middlewares;

namespace Rubius.DevSaunaB.DataServer.Extensions
{
    public static class WebSocketManagerExtensions
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<WebSocketConnectionManager>();

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }

        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder applicationBuilder,
            PathString path,
            WebSocketHandler handler)
        {
            return applicationBuilder.Map(path, (app) => app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }
    }
}
