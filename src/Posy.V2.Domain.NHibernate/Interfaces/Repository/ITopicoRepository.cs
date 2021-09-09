using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface ITopicoRepository : IRepository<Topico, int>
    {
        IEnumerable<Topico> GetTopicos(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
