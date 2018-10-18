using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotNetContrib.AspNetCore.Initialization.Internal
{
    internal class InitializationTaskRunner
    {
        private readonly ILogger<InitializationTaskRunner> _logger;
        private readonly IEnumerable<IInitializationTask> _initializers;

        public InitializationTaskRunner(
            ILogger<InitializationTaskRunner> logger,
            IEnumerable<IInitializationTask> initializers)
        {
            _logger = logger;
            _initializers = initializers;
        }

        public async Task InitializeAsync(InitializationContext context)
        {
            _logger.LogDebug("Starting initialization tasks...");

            try
            {
                foreach (var initializer in _initializers)
                {
                    _logger.LogDebug("Starting initialization task: {InitializerType}...", initializer.GetType());
                    try
                    {
                        await initializer.InitializeAsync(context);
                        _logger.LogDebug("Finished initialization task: {InitializerType}.", initializer.GetType());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An exception occurred while running initialization task: {InitializerType}.", initializer.GetType());
                        throw;
                    }
                }

                _logger.LogDebug("Finished initialization tasks.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while running initialization tasks.");
                throw;
            }
        }
    }
}