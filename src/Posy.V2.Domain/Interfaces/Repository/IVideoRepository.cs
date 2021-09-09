using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IVideoRepository : IRepository<Video, Guid>
    {
        IEnumerable<Video> Get(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
