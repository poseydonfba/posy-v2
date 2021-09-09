using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IVideoComentarioService : IDisposable
    {
        VideoComentario Comentar(Guid videoId, string comentario);
        void ExcluirComentario(Guid comentarioId);
        VideoComentario ObterComentario(Guid comentarioId);
        IEnumerable<VideoComentario> ObterComentarios(Guid videoId, int paginaAtual, int itensPagina);
    }
}
