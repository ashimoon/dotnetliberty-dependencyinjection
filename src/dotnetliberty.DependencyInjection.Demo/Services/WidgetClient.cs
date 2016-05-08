using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetliberty.DependencyInjection.Demo.Models;

namespace dotnetliberty.DependencyInjection.Demo.Services
{
    public class WidgetClient
    {
        private string _endpoint;

        /// <summary>
        /// Let's pretend we can't change this constructor and that we must
        /// use the static NewInstance method instead.
        /// </summary>
        /// <param name="endpoint"></param>
        private WidgetClient(string endpoint)
        {
            _endpoint = endpoint;
        }

        /// <summary>
        /// We have to call this static method to create a WidgetClient,
        /// but the dependency injection system will not be able to use this static method.
        /// 
        /// One option is to use a Func<IServiceProvider,object> instead, but since we need
        /// to look up the endpoint parameter from configuration too, it will start to get messy.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static WidgetClient NewInstance(string endpoint)
        {
            return new WidgetClient(endpoint);
        }

        public Widget FetchWidget(int id)
        {
            // fetch widget from REST endpoint
            return new Widget();
        }

        public void SaveWidget(Widget widget)
        {
            // save back to REST endpoint
        }
    }
}
