using Microsoft.AspNetCore.Builder;

namespace DotNetContrib.AspNetCore.Initialization
{
    public class DefaultSplashScreenStartup
    {
        public void Configure(IApplicationBuilder application)
        {
            application.UseStaticFiles();
            application.UseMiddleware<DefaultSplashScreenMiddleware>();
        }
    }
}