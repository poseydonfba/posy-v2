using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface ITopicoPostRepository : IRepository<TopicoPost, int>
    {
        IEnumerable<TopicoPost> GetPosts(int topicoId, int paginaAtual, int itensPagina, out int totalRecords, out TopicoPost ultimoPost);
    }
}
