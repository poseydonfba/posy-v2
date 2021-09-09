using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IConnectionRepository : IRepository<Connection, string>
    {
        IEnumerable<Connection> ObterAmigosConectados(Guid usuarioId);
        void ExcluirOldConnections(Guid usuarioId);
    }
}
