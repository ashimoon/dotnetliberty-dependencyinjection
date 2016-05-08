using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetliberty.DependencyInjection.Demo.Models;

namespace dotnetliberty.DependencyInjection.Demo.Services
{
    public class WidgetValidator
    {
        public bool IsValid(Widget widget)
        {
            return !string.IsNullOrEmpty(widget.Name);
        }
    }
}
