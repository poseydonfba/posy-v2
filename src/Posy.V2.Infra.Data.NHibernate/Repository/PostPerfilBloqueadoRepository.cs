using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PostPerfilBloqueadoRepository : Repository<PostPerfilBloqueado, int>, IPostPerfilBloqueadoRepository
    {
        public PostPerfilBloqueadoRepository(DatabaseContext context) : base(context)
        {

        }

        public PostPerfilBloqueado Get(int usuarioId, int usuarioIdBloqueado)
        {
            return DbSet.Where(x => x.UsuarioId == usuarioId && x.UsuarioIdBloqueado == usuarioIdBloqueado).FirstOrDefault();
        }
    }
}