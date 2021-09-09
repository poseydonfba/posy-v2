using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IConnectionService : IDisposable
    {
        void AddConnection(int connectionId, int usuarioId, string userAgent, bool connected);
        void ExcluirConnection(int connectionId, int usuarioId);
        void ExcluirOldConnections(int usuarioId);
        Connection ObterPorUsuario(int usuarioId, bool conected = true, params Expression<Func<Connection, object>>[] includeProperties);
        IEnumerable<Connection> ObterAmigosConectados(int usuarioId);
    }
}
