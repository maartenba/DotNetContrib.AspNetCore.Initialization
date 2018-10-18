# DotNetContrib.AspNetCore.Initialization

Add initialization tasks and an optional splash screen to ASP.NET Core web applications.

## Installation (NuGet)

	Install-Package DotNetContrib.AspNetCore.Initialization -Prerelease

## Usage

To add an initialization task, implement `IInitializationTask` and register it in your `Startup.ConfigureServices()`:

```csharp
public class SampleInitializationTask : IInitializationTask
{
    public async Task InitializeAsync(InitializationContext context)
    {
		// When using splash screen, report status
		context.Status.Message = "Reticulating splines...";
		
		await Task.Delay(TimeSpan.FromSeconds(30));
    }
}

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		// ...
		
		services.AddInitializationTask<SampleInitializationTask>();
		
		// ...
	}
}
```

Next, we will have to update our host to run initialization - either with o without a splash screen:

```csharp
public class Program
{
    private const bool UseLoadingScreen = true;
        
    public static async Task Main(string[] args)
    {
        var host = CreateWebHostBuilder(args).Build();
        if (UseLoadingScreen)
        {
            await host.InitializeWithSplashScreenAsync();
        }
        else
        {
            await host.InitializeAsync();
        }
        await host.RunAsync();
    }
        
    private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
}
```

## Customziing the splash screen

Add a file `splash.htm` in your `wwwroot`. Insert the string `{InitializationContext.Status.Message}` to render a splash message.

Sample `splash.htm`:

```
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Loading...</title>
    <style>
        body { font-family: "Helvetica Neue", Helvetica, Arial, sans-serif }
    </style>
    <script>
        setTimeout(function() {
            location.reload(true);
        }, 2500);
    </script>
</head>
<body>
<div style="text-align: center;">
    <img src="loading.svg" />
</div>
<div style="text-align: center;">
    <h1>Loading...</h1>
    <h2>{InitializationContext.Status.Message}</h2>
</div>
</body>
</html>
```