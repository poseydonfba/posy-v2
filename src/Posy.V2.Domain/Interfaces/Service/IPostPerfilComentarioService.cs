using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IPostPerfilComentarioService : IDisposable
    {
        PostPerfilComentario Comentar(Guid postId, string comentario);
        void ExcluirComentario(Guid comentarioId);
        PostPerfilComentario ObterComentario(Guid comentarioId);
        IEnumerable<PostPerfilComentario> ObterComentarios(Guid postId, int paginaAtual, int itensPagina);
    }
}
