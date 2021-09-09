using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Data.Entity;

namespace Posy.V2.Infra.Data.Repository
{
    public class PostOcultoRepository : Repository<PostOculto, Guid>, IPostOcultoRepository
    {
        public PostOcultoRepository(DatabaseContext context) : base(context)
        {

        }

        public void Ocultar(PostOculto post)
        {
            post.SetOcultar();

            _db.Entry(post).State = EntityState.Modified;
        }
    }
}