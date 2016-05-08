using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetliberty.DependencyInjection.Demo.Services;
using dotnetliberty.DependencyInjection;
using dotnetliberty.DependencyInjection.Demo.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;

namespace dotnetliberty.DependencyInjection.Demo.Factories
{
    public class WidgetClientFactory : IServiceFactory<WidgetClient>
    {
        private readonly IOptions<WidgetClientSettings> _options;
        private readonly ILogger<WidgetClientFactory> _logger;

        public WidgetClientFactory(IOptions<WidgetClientSettings> options, ILogger<WidgetClientFactory> logger)
        {
            _options = options;
            _logger = logger;
            _logger.LogInformation("Constructor called.");
        }

        public WidgetClient Build()
        {
            _logger.LogInformation($"About to build widget client with endpoint {_options.Value.Endpoint}");
            return WidgetClient.NewInstance(_options.Value.Endpoint);
        }
    }
}
