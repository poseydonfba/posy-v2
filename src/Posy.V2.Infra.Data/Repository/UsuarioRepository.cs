using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;

namespace Posy.V2.Infra.Data.Repository
{
    public class UsuarioRepository : Repository<Usuario, Guid>, IUsuarioRepository
    {
        public UsuarioRepository(DatabaseContext context) : base(context)
        {

        }

        public void DesativarLock(Guid id)
        {
            _db.Usuarios.Find(id).LockoutEnabled = false;
            _db.SaveChanges();
        }
    }
}