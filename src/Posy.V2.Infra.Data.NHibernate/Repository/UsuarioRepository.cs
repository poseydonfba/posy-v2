using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;

namespace Posy.V2.Infra.Data.Repository
{
    public class UsuarioRepository : Repository<Usuario, int>, IUsuarioRepository
    {
        public UsuarioRepository(DatabaseContext context) : base(context)
        {

        }

        public void DesativarLock(int id)
        {
            Get(id).LockoutEnabled = false;
            //Session.Update();
            //_db.SaveChanges();
        }
    }
}