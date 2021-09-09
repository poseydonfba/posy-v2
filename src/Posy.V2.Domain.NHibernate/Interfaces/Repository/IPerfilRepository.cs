using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IPerfilRepository : IRepository<Perfil, int>
    {
        Perfil GetByUsuario(int id);
        Perfil GetByAlias(string alias);
    }
}
