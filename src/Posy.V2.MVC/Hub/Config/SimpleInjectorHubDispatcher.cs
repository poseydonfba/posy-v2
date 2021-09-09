using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Lifestyles;

namespace Posy.V2.MVC.Hub.Config
{
    public class SimpleInjectorHubDispatcher : HubDispatcher
    {
        public SimpleInjectorHubDispatcher(Container container, HubConfiguration configuration)
            : base(configuration)
        {
            _container = container;
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            return Invoke(() => base.OnConnected(request, connectionId));
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return Invoke(() => base.OnReceived(request, connectionId, data));
        }

        protected override Task OnDisconnected(IRequest request, string connectionId,
            bool stopCalled)
        {
            return Invoke(() => base.OnDisconnected(request, connectionId, stopCalled));
        }

        protected override Task OnReconnected(IRequest request, string connectionId)
        {
            return Invoke(() => base.OnReconnected(request, connectionId));
        }

        private async Task Invoke(Func<Task> method)
        {
            //using (_container.BeginExecutionContextScope())
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
                await method();
        }

        private readonly Container _container;
    }
}