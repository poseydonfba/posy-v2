using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IPostPerfilService : IDisposable
    {
        PostPerfil Postar(string descricaoPost);
        void ExcluirPost(int postId);
        PostPerfil ObterPost(int postId);
        IEnumerable<PostPerfil> ObterPosts(int usuarioId, int paginaAtual, int itensPagina);
    }
}
