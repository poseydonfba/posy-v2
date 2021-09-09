using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IPostPerfilService : IDisposable
    {
        PostPerfil Postar(string descricaoPost);
        void ExcluirPost(Guid postId);
        PostPerfil ObterPost(Guid postId);
        IEnumerable<PostPerfil> ObterPosts(Guid usuarioId, int paginaAtual, int itensPagina);
    }
}
