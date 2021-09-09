using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class TopicoPostRepository : Repository<TopicoPost, Guid>, ITopicoPostRepository
    {
        public TopicoPostRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<TopicoPost> GetPosts(Guid topicoId, int paginaAtual, int itensPagina, out int totalRecords, out TopicoPost ultimoPost)
        {
            totalRecords = _db.TopicosPosts.Where(x => x.TopicoId == topicoId && x.Der == null).Count();

            ultimoPost = _db.TopicosPosts.Where(x => x.TopicoId == topicoId && x.Der == null).OrderByDescending(x => x.DataPost).FirstOrDefault();

            var posts = _db.TopicosPosts
                            .Include("Usuario.Perfil")
                            .Where(x => x.TopicoId == topicoId && x.Der == null)
                            .OrderBy(x => x.DataPost)
                            .Skip((paginaAtual - 1) * itensPagina)
                            .Take(itensPagina)
                            .ToList();

            return posts;
        }
    }
}