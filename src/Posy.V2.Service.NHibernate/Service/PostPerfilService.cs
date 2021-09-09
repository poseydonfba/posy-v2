using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class PostPerfilService : IPostPerfilService
    {
        private IPostPerfilRepository _repository;
        private IAmizadeService _amizadeService;
        private ICurrentUser _currentUser;
        private ICacheService _cacheService;

        public PostPerfilService(IPostPerfilRepository repository,
                                 IAmizadeService amizadeService,
                                 ICurrentUser currentUser,
                                 ICacheService cacheService)
        {
            _repository = repository;
            _amizadeService = amizadeService;
            _currentUser = currentUser;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Só posta para o usuario logado, evita de alguem postar para outra pessoa
        /// </summary>
        /// <param name="descricaoPost"></param>
        /// <returns></returns>
        public PostPerfil Postar(string descricaoPost)
        {
            var post = new PostPerfil(_currentUser.GetCurrentUserId(), descricaoPost);
            _repository.Insert(post);

            return post;
        }

        /// <summary>
        /// Só exclui um post se este post existir e for do usuario logado
        /// </summary>
        /// <param name="postId"></param>
        public void ExcluirPost(int postId)
        {
            var post = _repository.Get(postId);

            // Verifica se o post existe
            if (post == null)
                throw new Exception(Errors.PostInvalido);

            // Verifica se o dono do post é o usuario logado
            if (post.UsuarioId != _currentUser.GetCurrentUserId())
                throw new Exception(Errors.ExclusaoInvalida);

            post.Delete();
            _repository.Remove(post);
        }

        /// <summary>
        /// Só retorna um post se este for do usuario logado ou se for de
        /// uma pessoa que estiver na relação de amigos do usuario logado
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public PostPerfil ObterPost(int postId)
        {
            var post = _repository.GetNoTracking(postId);

            // Verifica se o post é do usuario logado
            if (post.UsuarioId == _currentUser.GetCurrentUserId())
                return post;

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), post.UsuarioId);

            // Não são amigos
            if (amizade != null)
                return post;

            return null;
        }

        /// <summary>
        /// Só retorna os posts se o usuarioId for do usuario logado ou se for de
        /// uma pessoa que estiver na relação de amigos do usuario logado
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <returns></returns>
        public IEnumerable<PostPerfil> ObterPosts(int usuarioId, int paginaAtual, int itensPagina)
        {
            if (usuarioId == _currentUser.GetCurrentUserId())
                return Posts(usuarioId, paginaAtual, itensPagina);

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), usuarioId);
            if (amizade != null)
                return Posts(usuarioId, paginaAtual, itensPagina);

            return new List<PostPerfil>();
        }
        // METODO 1
        //private IEnumerable<PostPerfil> Posts1(int usuarioId, int paginaAtual, int itensPagina)
        //{
        //    var key = $"{usuarioId}-{paginaAtual}-{itensPagina}";
        //    var list = _cacheService.Get<IEnumerable<PostPerfil>>(key);

        //    if (list == null)
        //    {
        //        list = _repository.Get(usuarioId, paginaAtual, itensPagina);
        //        _cacheService.Set(key, list);
        //    }

        //    return list;// _repository.Get(usuarioId, paginaAtual, itensPagina);
        //}

        // METODO 2
        private IEnumerable<PostPerfil> Posts(int usuarioId, int paginaAtual, int itensPagina)
        {
            //var list = _cacheService.GetOrSet(
            //    ()=> _repository.Get(usuarioId, paginaAtual, itensPagina), 
            //    new { id = usuarioId, page = paginaAtual, items = itensPagina },
            //    new TimeSpan(0, 10, 0));

            return _repository.Get(usuarioId, paginaAtual, itensPagina);// list;
        }

        public void Dispose()
        {
            _repository.Dispose();
            _amizadeService.Dispose();
        }
    }
}
