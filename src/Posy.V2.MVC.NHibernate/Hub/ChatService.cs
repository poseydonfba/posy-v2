using Microsoft.AspNet.SignalR.Hubs;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.MVC.Models;
using System;
using System.Linq;

namespace Posy.V2.MVC.Hub
{
    // https://gist.github.com/pmbanugo/99933fc8ec58e96e8e6a7f972ed9e5be
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConnectionService _connectionService;

        public ChatService(IUnitOfWork uow, IConnectionService connectionService)
        {
            _uow = uow;
            _connectionService = connectionService;
        }

        public void OnConnected(int userId, int connectionId, string userAgent, IHubCallerConnectionContext<dynamic> clients)
        {
            #region V1

            //var existingConnection = _connectionService.ObterPorUsuario(userId);
            //if (existingConnection == null)
            //{
            //    var userConnection = _connectionService.ObterPorUsuario(userId, true, x => x.Usuario.Perfil);
            //    var user = new
            //    {
            //        name = userConnection.Usuario.Perfil.Nome,
            //        id = userConnection.UsuarioId,
            //        connectionId = userConnection.ConnectionId,
            //        frase = userConnection.Usuario.Perfil.FraseHtml
            //    };

            //    clients.Others.userOnline(userConnection, DateTime.UtcNow.TimeOfDay); //notify other connected users
            //}
            //else
            //    _connectionService.ExcluirOldConnections(userId);

            //SaveUserConnection(userId, connectionId, userAgent);

            #endregion

            SaveUserConnection(userId, connectionId, userAgent);

            var userConnection = _connectionService.ObterPorUsuario(userId, true, x => x.Usuario.Perfil);
            if (userConnection != null)
            {
                var user = new ChatResponsePerfilModel
                {
                    Nome = userConnection.Usuario.Perfil.Nome,
                    Id = userConnection.UsuarioId,
                    ConnectionId = userConnection.Id,
                    Frase = userConnection.Usuario.Perfil.FraseHtml
                };

                clients.Others.userOnline(user, DateTime.UtcNow.TimeOfDay); //notify other connected users
            }        

            var onlineUsers = _connectionService.ObterAmigosConectados(userId).Select(x => new ChatResponsePerfilModel
            {
                Nome = x.Usuario.Perfil.Nome,
                Id = x.UsuarioId,
                ConnectionId = x.Id,
                Frase = x.Usuario.Perfil.FraseHtml
            });

            clients.Caller.setOnlineUsers(onlineUsers.ToList());
        }

        private void SaveUserConnection(int userId, int connectionId, string userAgent)
        {
            try
            {
                _connectionService.ExcluirOldConnections(userId);
                _uow.Commit();

                _connectionService.AddConnection(connectionId, userId, userAgent, true);
                _uow.Commit();
            }
            catch (Exception)
            {
                //TODO: Log exception
            }
        }

        public void OnDisconnected(int userId, int connectionId, IHubCallerConnectionContext<dynamic> clients)
        {
            try
            {
                _connectionService.ExcluirConnection(connectionId, userId);
                _uow.Commit();
            }
            catch (Exception)
            {
                //TODO: log exception
            }

            var existingConnection = _connectionService.ObterPorUsuario(userId);
            if (existingConnection == null)
            {
                //TODO: get username from db
                clients.All.userOffline(userId); //notify other connected users
            }
        }

        public void Broadcast(int connectionId, string message, string username, IHubCallerConnectionContext<dynamic> clients)
        {
            //var time = DateTime.Now.ToString("D") + " at " + DateTime.Now.ToString("h:mm:ss tt");
            var time = DateTime.UtcNow.ToLocalTime().ToString("HH:mm");

            if (connectionId <= 0)
                return;

            clients.Caller.addChatMessageEnviada(message, time); // Mostra a mensagem para o usuario que esta enviando (usuario logado)
            clients.Client(connectionId.ToString()).addChatMessageRecebida(username, message, time); // mostra a mensagem para o destino
            //clients.Others.addChatMessageRecebida(username, message, time);
            //clients.All.addChatMessageRecebida(username, message, time);
        }
    }
}