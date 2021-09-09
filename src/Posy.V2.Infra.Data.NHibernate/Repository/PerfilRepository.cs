using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PerfilRepository : Repository<Perfil, int>, IPerfilRepository
    {
        public PerfilRepository(DatabaseContext context) : base(context)
        {

        }

        public Perfil GetByUsuario(int usuarioId)
        {
            return DbSet.FirstOrDefault(x => x.Id == usuarioId);
        }

        public Perfil GetByAlias(string alias)
        {
            return DbSet.FirstOrDefault(x => x.Alias == alias);
        }
    }
}