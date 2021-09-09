using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class RecadoComentarioService : IRecadoComentarioService
    {
        private IRecadoComentarioRepository _repository;
        private IRecadoService _recadoService;
        private IPostPerfilBloqueadoService _postPerfilBloqueadoService;
        private IAmizadeService _amizadeService;
        private ICurrentUser _currentUser;

        public RecadoComentarioService(IRecadoComentarioRepository repository,
                                       IRecadoService recadoService,
                                       IPostPerfilBloqueadoService postPerfilBloqueadoService,
                                       IAmizadeService amizadeService,
                                       ICurrentUser currentUser)
        {
            _repository = repository;
            _recadoService = recadoService;
            _postPerfilBloqueadoService = postPerfilBloqueadoService;
            _amizadeService = amizadeService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 1 - Se o recado foi enviado para usuario logado
        /// 2 - Se o recado foi enviado pelo usuario logado
        /// 3 - Se seu perfil nao estiver bloqueado para postar 
        /// 4 - Se for de alguem que esteja na relação de amigos
        /// </summary>
        /// <param name="recadoId"></param>
        /// <param name="usuarioId"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        public RecadoComentario Comentar(Guid recadoId, string comentario)
        {
            var coment = new RecadoComentario(recadoId, _currentUser.GetCurrentUserId(), comentario);
            var recado = _recadoService.ObterRecado(recadoId);

            if (recado.EnviadoPorId != _currentUser.GetCurrentUserId() &&
                recado.EnviadoParaId != _currentUser.GetCurrentUserId())
                throw new Exception(Errors.ErroAoEnviarComentario);

            var usuarioComPostBloqueado = recado.EnviadoPorId == _currentUser.GetCurrentUserId() ? 
                null :
                _postPerfilBloqueadoService.ObterPerfilBloqueado(recado.EnviadoPorId, _currentUser.GetCurrentUserId());
            if (usuarioComPostBloqueado != null)
                throw new Exception(Errors.ErroAoEnviarComentario);

            var amizade = recado.EnviadoPorId == _currentUser.GetCurrentUserId() ?
                _amizadeService.ObterAmigo(recado.EnviadoParaId, _currentUser.GetCurrentUserId()) : 
                _amizadeService.ObterAmigo(recado.EnviadoPorId, _currentUser.GetCurrentUserId());
            if (amizade == null)
                throw new Exception(Errors.ErroAoEnviarComentario);

            _repository.Insert(coment);
            return coment;
        }

        /// <summary>
        /// 1 - Se o comentário for do usuario logado
        /// 2 - Quem enviou o recado em que foi comentado
        /// </summary>
        /// <param name="comentarioId"></param>
        public void ExcluirComentario(Guid comentarioId)
        {
            var comentario = ObterComentario(comentarioId);
            if (comentario.UsuarioId == _currentUser.GetCurrentUserId())
            {
                comentario.Delete(_currentUser.GetCurrentUserId());
                _repository.Remove(comentario);
                return;
            }

            var recado = _recadoService.ObterRecado(comentario.RecadoId);
            if (recado.EnviadoPorId == _currentUser.GetCurrentUserId())
            {
                comentario.Delete(_currentUser.GetCurrentUserId());
                _repository.Remove(comentario);
                return;
            }

            throw new Exception(Errors.ErroAoExcluirComentario);
        }

        public RecadoComentario ObterComentario(Guid comentarioId)
        {
            return _repository.Get(comentarioId);
        }

        /// <summary>
        /// 1 - Se o recado foi enviado para usuario logado
        /// 2 - Se o recado foi enviado pelo usuario logado
        /// 3 - Se o recado foi enviado para um amigo
        /// 4 - Se o recado foi enviado pelo amigo
        /// </summary>
        /// <param name="recadoId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <returns></returns>
        public IEnumerable<RecadoComentario> ObterComentarios(Guid recadoId, int paginaAtual, int itensPagina)
        {
            var recado = _recadoService.ObterRecado(recadoId);

            if (recado.EnviadoPorId == _currentUser.GetCurrentUserId() ||
                recado.EnviadoParaId == _currentUser.GetCurrentUserId())
                return _repository.Get(recadoId, paginaAtual, itensPagina);

            var amizade = recado.EnviadoPorId == _currentUser.GetCurrentUserId() ?
                _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), recado.EnviadoParaId) :
                _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), recado.EnviadoPorId);
            if (amizade != null)
                return _repository.Get(recadoId, paginaAtual, itensPagina);

            return new List<RecadoComentario>();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _recadoService.Dispose();
            _postPerfilBloqueadoService.Dispose();
            _amizadeService.Dispose();
        }
    }
}
