using Microsoft.AspNet.SignalR.Hubs;
using SimpleInjector;

namespace Posy.V2.MVC.Hub.Config
{
    public class SimpleInjectorHubActivator : IHubActivator
    {
        public SimpleInjectorHubActivator(Container container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)_container.GetInstance(descriptor.HubType);
        }

        private readonly Container _container;
    }
}