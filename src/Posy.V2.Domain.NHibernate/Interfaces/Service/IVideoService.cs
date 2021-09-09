using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IVideoService : IDisposable
    {
        Video SalvarVideo(string url, string nomeVideo);
        void ExcluirVideo(int videoId);
        Video ObterVideo(int videoId);
        IEnumerable<Video> ObterVideos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
