using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IPostPerfilComentarioRepository : IRepository<PostPerfilComentario, int>
    {
        IEnumerable<PostPerfilComentario> Get(int postId, int paginaAtual, int itensPagina);
    }
}
