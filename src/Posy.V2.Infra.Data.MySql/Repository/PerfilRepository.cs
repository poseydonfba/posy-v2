using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PerfilRepository : Repository<Perfil, Guid>, IPerfilRepository
    {
        public PerfilRepository(DatabaseContext context) : base(context)
        {

        }

        public Perfil GetByUsuario(Guid usuarioId)
        {
            return _db.Perfis.FirstOrDefault(x => x.UsuarioId == usuarioId);
        }

        public Perfil GetByAlias(string alias)
        {
            return _db.Perfis.FirstOrDefault(x => x.Alias == alias);
        }
    }
}