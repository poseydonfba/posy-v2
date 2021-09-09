using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IModeradorRepository : IRepository<Moderador, int>
    {
        Moderador Get(int comunidadeId, int usuarioId);
        IEnumerable<Moderador> GetModeradores(int comunidadeId);
        IEnumerable<Moderador> GetModeradores(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Moderador> GetComunidades(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
