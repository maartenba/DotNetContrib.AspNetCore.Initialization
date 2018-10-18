using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace DotNetContrib.AspNetCore.Initialization
{
    public class DefaultSplashScreenMiddleware
    {
        private readonly string[] _defaultFileNames = {
            "splash.htm",
            "splash.html",
            "loader.htm",
            "loader.html"
        };
      
        private readonly RequestDelegate _next;
        private readonly InitializationContext _initializationContext;
        private readonly IFileProvider _fileProvider;

        public DefaultSplashScreenMiddleware(RequestDelegate next, IHostingEnvironment hostingEnvironment, InitializationContext initializationContext)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _initializationContext = initializationContext;
            _fileProvider = hostingEnvironment?.WebRootFileProvider ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        public Task Invoke(HttpContext context)
        {
            if (string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase) && context.Request.Path == "/")
            {
                foreach (var defaultFileName in _defaultFileNames)
                {
                    var defaultFile = _fileProvider.GetFileInfo(defaultFileName);
                    if (defaultFile.Exists)
                    {
                        return SplashScreenFromStream(context, defaultFile.CreateReadStream());
                    }
                }
                
                return SplashScreenFromStream(context, GetType().Assembly.GetManifestResourceStream("DotNetContrib.AspNetCore.Initialization.Resources.splash.htm"));
            } 
            if (string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase) && context.Request.Path == "/loading.svg")
            {
                using (var stream = GetType().Assembly.GetManifestResourceStream("DotNetContrib.AspNetCore.Initialization.Resources.loading.svg"))
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "image/svg+xml";
                    stream.CopyTo(context.Response.Body);
                    return Task.CompletedTask;
                }
            }

            return _next(context);
        }

        private Task SplashScreenFromStream(HttpContext context, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var html = reader.ReadToEnd();
                html = html.Replace("{InitializationContext.Status.Message}", _initializationContext.Status.Message);

                context.Response.StatusCode = 200;
                return context.Response.WriteAsync(html);
            }
        }
    }
}