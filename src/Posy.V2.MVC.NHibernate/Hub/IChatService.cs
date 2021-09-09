using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace Posy.V2.MVC.Hub
{
    public interface IChatService
    {
        void OnConnected(int userId, int connectionId, string userAgent, IHubCallerConnectionContext<dynamic> clients); //HubConnectionContext
        void OnDisconnected(int userId, int connectionId, IHubCallerConnectionContext<dynamic> clients);
        void Broadcast(int connectionId, string message, string username, IHubCallerConnectionContext<dynamic> clients); // IHubCallerConnectionContext<dynamic>
    }
}
