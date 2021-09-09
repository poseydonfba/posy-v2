using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IVisitantePerfilRepository : IRepository<VisitantePerfil, Guid>
    {
        IEnumerable<Perfil> GetVisitantes(Guid usuarioId);
        IEnumerable<Perfil> GetVisitantes(Guid usuarioId, int take);
        IEnumerable<Perfil> GetVisitados(Guid usuarioId);
        IEnumerable<Perfil> GetVisitados(Guid usuarioId, int take);
    }
}
