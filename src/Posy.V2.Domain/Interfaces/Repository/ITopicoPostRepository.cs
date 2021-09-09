using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface ITopicoPostRepository : IRepository<TopicoPost, Guid>
    {
        IEnumerable<TopicoPost> GetPosts(Guid topicoId, int paginaAtual, int itensPagina, out int totalRecords, out TopicoPost ultimoPost);
    }
}
