using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IModeradorRepository : IRepository<Moderador, Guid>
    {
        Moderador Get(Guid comunidadeId, Guid usuarioId);
        IEnumerable<Moderador> GetModeradores(Guid comunidadeId);
        IEnumerable<Moderador> GetModeradores(Guid comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Moderador> GetComunidades(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
