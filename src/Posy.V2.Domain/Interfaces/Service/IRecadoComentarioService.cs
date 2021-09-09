using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IRecadoComentarioService : IDisposable
    {
        RecadoComentario Comentar(Guid recadoId, string comentario);
        void ExcluirComentario(Guid comentarioId);
        RecadoComentario ObterComentario(Guid comentarioId);
        IEnumerable<RecadoComentario> ObterComentarios(Guid recadoId, int paginaAtual, int itensPagina);
    }
}
