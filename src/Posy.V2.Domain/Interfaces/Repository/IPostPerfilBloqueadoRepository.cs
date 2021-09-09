using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IPostPerfilBloqueadoRepository : IRepository<PostPerfilBloqueado, Guid>
    {
        PostPerfilBloqueado Get(Guid usuarioId, Guid usuarioIdBloqueado);
    }
}
