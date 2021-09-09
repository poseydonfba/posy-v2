using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class PostPerfilComentarioService : IPostPerfilComentarioService
    {
        private IPostPerfilComentarioRepository _repository;
        private IPostPerfilService _postPerfilService;
        private IPostPerfilBloqueadoService _postPerfilBloqueadoService;
        private IAmizadeService _amizadeService;
        private ICurrentUser _currentUser;

        public PostPerfilComentarioService(IPostPerfilComentarioRepository repository,
                                           IPostPerfilService postPerfilService,
                                           IPostPerfilBloqueadoService postPerfilBloqueadoService,
                                           IAmizadeService amizadeService,
                                           ICurrentUser currentUser)
        {
            _repository = repository;
            _postPerfilService = postPerfilService;
            _postPerfilBloqueadoService = postPerfilBloqueadoService;
            _amizadeService = amizadeService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 1 - Este for do usuarioId;
        /// 2 - Se seu perfil nao estiver bloqueado para postar; 
        /// 3 - Se for de alguem que esteja na relação de amigos;
        /// </summary>
        /// <param name="postId">Post a ser comentado</param>
        /// <param name="usuarioId">Usuario que esta comentando</param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        public PostPerfilComentario Comentar(Guid postId, string comentario)
        {
            var coment = new PostPerfilComentario(postId, _currentUser.GetCurrentUserId(), comentario);
            var post = _postPerfilService.ObterPost(postId);

            if (post.UsuarioId == _currentUser.GetCurrentUserId())
            {
                _repository.Insert(coment);
                return coment;
            }

            var usuarioComPostBloqueado = _postPerfilBloqueadoService.ObterPerfilBloqueado(post.UsuarioId, _currentUser.GetCurrentUserId());
            if (usuarioComPostBloqueado != null)
                throw new Exception(Errors.ErroAoEnviarComentario);

            var amizade = _amizadeService.ObterAmigo(post.UsuarioId, _currentUser.GetCurrentUserId());
            if (amizade == null)
                throw new Exception(Errors.ErroAoEnviarComentario);

            _repository.Insert(coment);
            return coment;
        }

        /// <summary>
        /// 1 - Se o comentário for do usuario logado
        /// 2 - Quem for o dono do post em que foi comentado
        /// </summary>
        /// <param name="comentarioId"></param>
        /// <param name="usuarioIdExclusao"></param>
        public void ExcluirComentario(Guid comentarioId)
        {
            var comentario = ObterComentario(comentarioId);

            // Verifica se o comentário foi criado pelo usuario logado
            if (comentario.UsuarioId == _currentUser.GetCurrentUserId())
            {
                comentario.Delete(_currentUser.GetCurrentUserId());
                _repository.Remove(comentario);
                return;
            }

            var post = _postPerfilService.ObterPost(comentario.PostPerfilId);

            // Verifica se o post do comentário foi criado pelo usuario logado
            if (post.UsuarioId == _currentUser.GetCurrentUserId())
            {
                comentario.Delete(_currentUser.GetCurrentUserId());
                _repository.Remove(comentario);
                return;
            }

            throw new Exception(Errors.ErroAoExcluirComentario);
        }

        public PostPerfilComentario ObterComentario(Guid comentarioId)
        {
            return _repository.Get(comentarioId);
        }

        /// <summary>
        /// 1 - Se o post for do usuario logado
        /// 2 - Se o post for de um amigo
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <returns></returns>
        public IEnumerable<PostPerfilComentario> ObterComentarios(Guid postId, int paginaAtual, int itensPagina)
        {
            var post = _postPerfilService.ObterPost(postId);

            if (post.UsuarioId == _currentUser.GetCurrentUserId())
                return _repository.Get(postId, paginaAtual, itensPagina);

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), post.UsuarioId);
            if (amizade != null)
                return _repository.Get(postId, paginaAtual, itensPagina);

            return new List<PostPerfilComentario>();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _postPerfilService.Dispose();
            _postPerfilBloqueadoService.Dispose();
            _amizadeService.Dispose();
        }
    }
}
