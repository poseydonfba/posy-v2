using Microsoft.AspNet.Identity;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Posy.V2.WF.Hub
{
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly Container _container;

        public ChatHub(Container container)
        {
            _container = container;
        }

        public override Task OnConnected()
        {
            var userId = Guid.Parse(Context.User.Identity.GetUserId());
            _container.GetInstance<IChatService>().OnConnected(userId, Context.ConnectionId, Context.Request.Headers["User-Agent"], Clients);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //using (_container.BeginExecutionContextScope())
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                var userId = Guid.Parse(Context.User.Identity.GetUserId());
                _container.GetInstance<IChatService>().OnDisconnected(userId, Context.ConnectionId, Clients);
                return base.OnDisconnected(stopCalled);
            }
        }

        public void Broadcast(string connectionId, string message)
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            var nome = identity.FindFirstValue("posy:nome");
            _container.GetInstance<IChatService>().Broadcast(connectionId, message, nome, Clients);
        }
    }
}