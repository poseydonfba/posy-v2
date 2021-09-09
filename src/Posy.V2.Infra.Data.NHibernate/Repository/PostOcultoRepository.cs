using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;

namespace Posy.V2.Infra.Data.Repository
{
    public class PostOcultoRepository : Repository<PostOculto, int>, IPostOcultoRepository
    {
        public PostOcultoRepository(DatabaseContext context) : base(context)
        {

        }

        public void Ocultar(PostOculto post)
        {
            post.SetOcultar();

            Session.Update(post);
            //_db.Entry(post).State = EntityState.Modified;
        }
    }
}