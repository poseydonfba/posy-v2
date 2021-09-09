using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Domain.Interfaces.Service;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using System;
using System.Collections.Generic;

namespace Posy.V2.Service
{
    public class VideoService : IVideoService
    {
        private IVideoRepository _repository;
        private IAmizadeService _amizadeService;
        private ICurrentUser _currentUser;

        public VideoService(IVideoRepository repository,
                            IAmizadeService amizadeService,
                            ICurrentUser currentUser)
        {
            _repository = repository;
            _amizadeService = amizadeService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="nomeVideo"></param>
        /// <returns></returns>
        public Video SalvarVideo(string url, string nomeVideo)
        {
            var video = new Video(_currentUser.GetCurrentUserId(), url, nomeVideo);

            _repository.Insert(video);

            return video;
        }

        /// <summary>
        /// 1 - Se for um video do usuario logado
        /// </summary>
        /// <param name="videoId"></param>
        public void ExcluirVideo(Guid videoId)
        {
            var video = ObterVideo(videoId);
            if (video.UsuarioId != _currentUser.GetCurrentUserId())
                throw new Exception(Errors.ExclusaoInvalida);

            video.Delete();

            _repository.Remove(video);
        }

        public Video ObterVideo(Guid videoId)
        {
            return _repository.Get(videoId);
        }

        /// <summary>
        /// 1 - Se for do usuario logado
        /// 2 - Se for de uma amigo
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="itensPagina"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Video> ObterVideos(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            if (usuarioId == _currentUser.GetCurrentUserId())
                return _repository.Get(usuarioId, paginaAtual, itensPagina, out totalRecords);

            var amizade = _amizadeService.ObterAmigo(_currentUser.GetCurrentUserId(), usuarioId);
            if (amizade != null)
                return _repository.Get(usuarioId, paginaAtual, itensPagina, out totalRecords);

            totalRecords = 0;
            return new List<Video>();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _amizadeService.Dispose();
        }
    }
}
