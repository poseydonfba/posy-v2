using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IConnectionService : IDisposable
    {
        void AddConnection(string connectionId, Guid usuarioId, string userAgent, bool connected);
        void ExcluirConnection(string connectionId, Guid usuarioId);
        void ExcluirOldConnections(Guid usuarioId);
        Connection ObterPorUsuario(Guid usuarioId, bool conected = true, params Expression<Func<Connection, object>>[] includeProperties);
        IEnumerable<Connection> ObterAmigosConectados(Guid usuarioId);
    }
}
