using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IRecadoComentarioRepository : IRepository<RecadoComentario, int>
    {
        IEnumerable<RecadoComentario> Get(int recadoId, int paginaAtual, int itensPagina);
    }
}
