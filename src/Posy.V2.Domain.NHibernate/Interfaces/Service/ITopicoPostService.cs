using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface ITopicoPostService : IDisposable
    {
        TopicoPost SalvarPost(int topicoId, string descricao);
        void ExcluirTopicoPost(int postId);
        void ExcluirTopicoPostPermanente(int postId);
        TopicoPost Obter(int postId);
        IEnumerable<TopicoPost> ObterPosts(int topicoId, int paginaAtual, int itensPagina, out int totalRecords, out TopicoPost ultimoPost);
    }
}
