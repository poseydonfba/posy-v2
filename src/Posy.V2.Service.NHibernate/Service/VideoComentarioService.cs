using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class VideoComentarioService : IVideoComentarioService
    {
        private IVideoComentarioRepository _repository;
        private IVideoService _videoService;
        private IAmizadeService _amizadeService;
        private ICurrentUser _currentUser;

        public VideoComentarioService(IVideoComentarioRepository repository,
                                      IVideoService videoService,
                                      IAmizadeService amizadeService,
                                      ICurrentUser currentUser)
        {
            _repository = repository;
            _videoService = videoService;
            _amizadeService = amizadeService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        public VideoComentario Comentar(int videoId, string comentario)
        {
            var coment = new VideoComentario(videoId, _currentUser.GetCurrentUserId(), comentario);

            _repository.Insert(coment);

            return coment;
        }

        /// <summary>
        /// 1 - Se o comentário for do usuario logado
        /// 2 - Se o for o dono do video
        /// </summary>
        /// <param name="comentarioId"></param>
        public void ExcluirComentario(int comentarioId)
        {
            var comentario = ObterComentario(comentarioId);
            if (comentario.UsuarioId == _currentUser.GetCurrentUserId())
            {
                comentario.Delete(_currentUser.GetCurrentUserId());
                _repository.Remove(comentario);
                return;
            }

            var video = _videoService.ObterVideo(comentario.VideoId);
            if (video.UsuarioId == _currentUser.GetCurrentUserId())
            {
                comentario.Delete(_currentUser.GetCurrentUserId());
                _repository.Remove(comentario);
                return;
            }

            throw new Exception(Errors.ErroAoExcluirComentario);
        }

        public VideoComentario ObterComentario(int comentarioId)
        {
            return _repository.Get(comentarioId);
        }

        /// <summary>
        /// 1 - Se o video for do usuario logado
        /// 2 - Se o video for de um amigo
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <returns></returns>
        public IEnumerable<VideoComentario> ObterComentarios(int videoId, int paginaAtual, int itensPagina)
        {
            var video = _videoService.ObterVideo(videoId);

            if (video.UsuarioId == _currentUser.GetCurrentUserId())
                return _repository.Get(videoId, paginaAtual, itensPagina);

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), video.UsuarioId);
            if (amizade != null)
                return _repository.Get(videoId, paginaAtual, itensPagina);

            return new List<VideoComentario>();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _videoService.Dispose();
            _amizadeService.Dispose();
        }
    }
}
