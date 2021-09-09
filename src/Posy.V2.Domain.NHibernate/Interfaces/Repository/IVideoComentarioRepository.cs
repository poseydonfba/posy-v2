using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IVideoComentarioRepository : IRepository<VideoComentario, int>
    {
        IEnumerable<VideoComentario> Get(int videoId, int paginaAtual, int itensPagina);
    }
}
