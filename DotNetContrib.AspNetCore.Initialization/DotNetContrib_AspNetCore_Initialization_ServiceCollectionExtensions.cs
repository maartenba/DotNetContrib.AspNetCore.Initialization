using System;
using System.Threading.Tasks;
using DotNetContrib.AspNetCore.Initialization;
using DotNetContrib.AspNetCore.Initialization.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static class DotNetContrib_AspNetCore_Initialization_ServiceCollectionExtensions
    {
        private static IServiceCollection AddInitializationTasksRunner(this IServiceCollection services)
        {
            services.TryAddTransient<InitializationTaskRunner>();
            return services;
        }

        public static IServiceCollection AddInitializationTask<TInitializer>(this IServiceCollection services)
            where TInitializer : class, IInitializationTask
        {
            return services
                .AddInitializationTasksRunner()
                .AddTransient<IInitializationTask, TInitializer>();
        }

        public static IServiceCollection AddInitializationTask<TInitializer>(this IServiceCollection services, TInitializer initializer)
            where TInitializer : class, IInitializationTask
        {
            return services
                .AddInitializationTasksRunner()
                .AddSingleton<IInitializationTask>(initializer);
        }

        public static IServiceCollection AddInitializationTask(this IServiceCollection services, Func<IServiceProvider, IInitializationTask> implementationFactory)
        {
            return services
                .AddInitializationTasksRunner()
                .AddTransient(implementationFactory);
        }

        public static IServiceCollection AddInitializationTask(this IServiceCollection services, Type initializerType)
        {
            return services
                .AddInitializationTasksRunner()
                .AddTransient(typeof(IInitializationTask), initializerType);
        }
    }
}