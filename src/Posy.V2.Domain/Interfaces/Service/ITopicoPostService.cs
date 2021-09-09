using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface ITopicoPostService : IDisposable
    {
        TopicoPost SalvarPost(Guid topicoId, string descricao);
        void ExcluirTopicoPost(Guid postId);
        void ExcluirTopicoPostPermanente(Guid postId);
        TopicoPost Obter(Guid postId);
        IEnumerable<TopicoPost> ObterPosts(Guid topicoId, int paginaAtual, int itensPagina, out int totalRecords, out TopicoPost ultimoPost);
    }
}
