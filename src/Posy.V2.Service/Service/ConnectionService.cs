using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Posy.V2.Service
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConnectionRepository _connectionRepository;

        public ConnectionService(IConnectionRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        public void AddConnection(string connectionId, Guid usuarioId, string userAgent, bool connected)
        {
            var connection = new Connection
            {
                ConnectionId = connectionId,
                UsuarioId = usuarioId,
                UserAgent = userAgent,
                Connected = connected,
                DataConnected = ConfigurationBase.DataAtual
            };

            _connectionRepository.Insert(connection);
        }

        public void ExcluirConnection(string connectionId, Guid usuarioId)
        {
            var connection = _connectionRepository.Get(x => x.ConnectionId == connectionId && x.UsuarioId == usuarioId).FirstOrDefault();
            if (connection != null)
            {
                connection.Connected = false;
                connection.DataDisconnected = ConfigurationBase.DataAtual;
                connection.TipoDesconexao = TipoDesconexao.AUTOMATICA;
                _connectionRepository.Remove(connection);
            }
        }

        public void ExcluirOldConnections(Guid usuarioId)
        {
            _connectionRepository.ExcluirOldConnections(usuarioId);
        }

        public Connection ObterPorUsuario(Guid usuarioId, bool conected = true, params Expression<Func<Connection, object>>[] includeProperties)
        {
            //Expression<Func<Connection, object>> e1 = t => t.Usuario.Perfil;

            return _connectionRepository.Get(x => x.UsuarioId == usuarioId && x.Connected == conected, null, includeProperties /*t => t.Usuario.Perfil*/).FirstOrDefault();
        }

        public IEnumerable<Connection> ObterAmigosConectados(Guid usuarioId)
        {
            return _connectionRepository.ObterAmigosConectados(usuarioId);
        }

        public void Dispose()
        {
            _connectionRepository.Dispose();
        }
    }
}
