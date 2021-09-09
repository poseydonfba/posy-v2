using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PostPerfilBloqueadoRepository : Repository<PostPerfilBloqueado, Guid>, IPostPerfilBloqueadoRepository
    {
        public PostPerfilBloqueadoRepository(DatabaseContext context) : base(context)
        {

        }

        public PostPerfilBloqueado Get(Guid usuarioId, Guid usuarioIdBloqueado)
        {
            return _db.PostsPerfilBloqueado.Where(x => x.UsuarioId == usuarioId && x.UsuarioIdBloqueado == usuarioIdBloqueado).FirstOrDefault();
        }
    }
}