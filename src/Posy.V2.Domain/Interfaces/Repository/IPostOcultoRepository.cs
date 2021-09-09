using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IPostOcultoRepository : IRepository<PostOculto, Guid>
    {
        void Ocultar(PostOculto post);
    }
}
