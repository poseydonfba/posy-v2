using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IRecadoComentarioService : IDisposable
    {
        RecadoComentario Comentar(int recadoId, string comentario);
        void ExcluirComentario(int comentarioId);
        RecadoComentario ObterComentario(int comentarioId);
        IEnumerable<RecadoComentario> ObterComentarios(int recadoId, int paginaAtual, int itensPagina);
    }
}
