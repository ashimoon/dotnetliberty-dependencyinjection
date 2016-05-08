using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace dotnetliberty.DependencyInjection.Demo.Services
{
    public class WidgetService
    {
        private readonly WidgetClient _client;
        private readonly WidgetValidator _validator;
        private readonly ILogger<WidgetService> _logger;

        public WidgetService(WidgetClient client, WidgetValidator validator, ILogger<WidgetService> logger)
        {
            _client = client;
            _validator = validator;
            _logger = logger;
            _logger.LogInformation("Constructor called.");
        }

        public void UpdateWidgetName(int widgetId, string name)
        {
            _logger.LogInformation($"Updating widget with id {widgetId} to name {name}");
            var widget = _client.FetchWidget(widgetId);
            widget.Name = name;
            if (!_validator.IsValid(widget))
            {
                var e = new InvalidOperationException("Cannot save invalid widget.");
                _logger.LogError("Error updating widget name.", e);
                throw e;
            }
            _logger.LogInformation("Saving widget.");
            _client.SaveWidget(widget);
        }
    }
}
