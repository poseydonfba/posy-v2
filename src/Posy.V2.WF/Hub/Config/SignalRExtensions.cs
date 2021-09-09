using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SimpleInjector;

namespace Posy.V2.WF.Hub.Config
{
    public static class SignalRExtensions
    {
        public static void EnableSignalR(this Container container)
        {
            container.EnableSignalR(new HubConfiguration());
        }

        public static void EnableSignalR(this Container container, HubConfiguration config)
        {
            var hubActivator = new SimpleInjectorHubActivator(container);

            config.Resolver.Register(typeof(SimpleInjectorHubDispatcher),
                () => new SimpleInjectorHubDispatcher(container, config));

            config.Resolver.Register(typeof(IHubActivator), () => hubActivator);
        }
    }
}