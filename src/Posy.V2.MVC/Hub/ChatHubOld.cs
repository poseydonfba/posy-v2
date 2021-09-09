using Microsoft.AspNet.Identity;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.MVC.Controllers;
using SimpleInjector;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Posy.V2.MVC.Hub
{
    //// Artigos
    //// https://docs.microsoft.com/pt-br/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr-and-mvc
    //// https://docs.microsoft.com/pt-br/aspnet/signalr/overview/security/hub-authorization
    public class ChatHubOld : Microsoft.AspNet.SignalR.Hub
    {
        //    //private readonly Container _container;

        //    //public ChatHub(Container container)
        //    //{
        //    //    _container = container;
        //    //}

        //    //public override Task OnConnected()
        //    //{
        //    //    _container.GetInstance<IChatService>().OnConnected();
        //    //    return base.OnConnected();
        //    //}

        //    //public override Task OnDisconnected(bool stopCalled)
        //    //{
        //    //    using (_container.BeginExecutionContextScope())
        //    //    {
        //    //        _container.GetInstance<IChatService>().OnDisconnected();
        //    //        return base.OnDisconnected();
        //    //    }
        //    //    return base.OnDisconnected(stopCalled);
        //    //}

        //    //public void Broadcast(string message)
        //    //{
        //    //    _container.GetInstance<IChatService>().Broadcast(message);
        //    //    Clients.All.newMessage(message, username, time);
        //    //}

        private readonly IUnitOfWork _uow;
        private readonly IConnectionService _connectionService;

        public ChatHubOld(IUnitOfWork uow, IConnectionService connectionService)
        {
            _uow = uow;
            _connectionService = connectionService;
        }

        public void SendChatMessage(string who, string message)
        {
            var perfilLogado = BaseControllerPerfil.PerfilLogged;
            if (perfilLogado == null)
                perfilLogado = BaseControllerComunidade.PerfilLogged;

            if (perfilLogado == null)
            {
                Clients.Caller.showErrorMessage("Could not find that user.");
                return;
            }

            var whoGuid = Guid.Parse(who);

            var time = DateTime.UtcNow.ToLocalTime().ToString("HH:mm");

            //Clients.Caller.addChatMessageEnviada(message, time);

            //var perfisConectados = _connectionService.ObterUsuariosConectados(whoGuid);

            //if (!perfisConectados.Any())
            //{
            //    Clients.Caller.showErrorMessage("The user is no longer connected.");
            //}
            //else
            //{
            //    foreach (var connection in perfisConectados)
            //    {
            //        Clients.Client(connection.ConnectionId)
            //            .addChatMessage(perfilLogado.Nome + " " + perfilLogado.Sobrenome, message, time);
            //    }
            //}
        }

        public override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var usuarioId = Guid.Parse(Context.User.Identity.GetUserId());

                using (_uow)
                {
                    _connectionService.AddConnection(Context.ConnectionId, usuarioId, Context.Request.Headers["User-Agent"], true);
                    _uow.Commit();
                }

            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            using (_uow)
            {
                //_connectionService.ExcluirConnection(Context.ConnectionId);
                _uow.Commit();
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}