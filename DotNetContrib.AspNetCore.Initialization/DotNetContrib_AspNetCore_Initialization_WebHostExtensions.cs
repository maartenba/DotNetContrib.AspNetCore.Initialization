using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetContrib.AspNetCore.Initialization;
using DotNetContrib.AspNetCore.Initialization.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Hosting
{
    // ReSharper disable once InconsistentNaming
    public static class DotNetContrib_AspNetCore_Initialization_WebHostExtensions
    {
        public static Task InitializeAsync(this IWebHost host)
        {
            return InitializeAsync(host, new InitializationContext());
        }

        public static async Task InitializeWithSplashScreenAsync(this IWebHost host, string[] args = null)
        {
            var context = new InitializationContext();
            
            var splashScreenHost = WebHost.CreateDefaultBuilder(args ?? new string[0])
                .Configure(application =>
                {
                    application.UseStaticFiles();
                    application.UseMiddleware<DefaultSplashScreenMiddleware>(context);
                })
                .Build();

            await Task.WhenAny(
                splashScreenHost.RunAsync(),
                InitializeAsync(host, context));
            
            await splashScreenHost.StopAsync();
        }
        
        private static async Task InitializeAsync(IWebHost host, InitializationContext context)
        {
            using (var scope = host.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetService<InitializationTaskRunner>();
                if (initializer != null)
                {
                    await initializer.InitializeAsync(context);
                }
            }
        }
    }
}