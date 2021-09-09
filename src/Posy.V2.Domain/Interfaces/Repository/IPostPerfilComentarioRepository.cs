using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IPostPerfilComentarioRepository : IRepository<PostPerfilComentario, Guid>
    {
        IEnumerable<PostPerfilComentario> Get(Guid postId, int paginaAtual, int itensPagina);
    }
}
