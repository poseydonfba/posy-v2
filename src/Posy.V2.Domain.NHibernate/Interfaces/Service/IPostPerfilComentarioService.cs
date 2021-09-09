using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IPostPerfilComentarioService : IDisposable
    {
        PostPerfilComentario Comentar(int postId, string comentario);
        void ExcluirComentario(int comentarioId);
        PostPerfilComentario ObterComentario(int comentarioId);
        IEnumerable<PostPerfilComentario> ObterComentarios(int postId, int paginaAtual, int itensPagina);
    }
}
