using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IPostPerfilRepository : IRepository<PostPerfil, Guid>
    {
        IEnumerable<PostPerfil> Get(Guid usuarioId, int paginaAtual, int itensPagina);
    }
}
