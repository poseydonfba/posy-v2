using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IRecadoComentarioRepository : IRepository<RecadoComentario, Guid>
    {
        IEnumerable<RecadoComentario> Get(Guid recadoId, int paginaAtual, int itensPagina);
    }
}
