using Posy.V2.Domain.Entities;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IVisitantePerfilRepository : IRepository<VisitantePerfil, int>
    {
        IEnumerable<Perfil> GetVisitantes(int usuarioId);
        IEnumerable<Perfil> GetVisitantes(int usuarioId, int take);
        IEnumerable<Perfil> GetVisitados(int usuarioId);
        IEnumerable<Perfil> GetVisitados(int usuarioId, int take);
    }
}
