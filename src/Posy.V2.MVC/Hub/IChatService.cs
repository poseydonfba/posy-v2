using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace Posy.V2.MVC.Hub
{
    public interface IChatService
    {
        void OnConnected(Guid userId, string connectionId, string userAgent, IHubCallerConnectionContext<dynamic> clients); //HubConnectionContext
        void OnDisconnected(Guid userId, string connectionId, IHubCallerConnectionContext<dynamic> clients);
        void Broadcast(string connectionId, string message, string username, IHubCallerConnectionContext<dynamic> clients); // IHubCallerConnectionContext<dynamic>
    }
}
