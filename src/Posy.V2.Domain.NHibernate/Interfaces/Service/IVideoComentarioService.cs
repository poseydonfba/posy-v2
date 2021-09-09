using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IVideoComentarioService : IDisposable
    {
        VideoComentario Comentar(int videoId, string comentario);
        void ExcluirComentario(int comentarioId);
        VideoComentario ObterComentario(int comentarioId);
        IEnumerable<VideoComentario> ObterComentarios(int videoId, int paginaAtual, int itensPagina);
    }
}
