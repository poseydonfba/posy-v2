using NHibernate.Linq;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PostPerfilComentarioRepository : Repository<PostPerfilComentario, int>, IPostPerfilComentarioRepository
    {
        public PostPerfilComentarioRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<PostPerfilComentario> Get(int postId, int paginaAtual, int itensPagina)
        {
            if (itensPagina == 0)
                return DbSet
                        .Where(x => x.PostPerfilId == postId && x.Der == null)
                        .OrderByDescending(x => x.Data)
                        .Fetch(x => x.Usuario)
                        .ThenFetch(x => x.Perfil)
                        .ToList();
            else
                return DbSet
                        .Where(x => x.PostPerfilId == postId && x.Der == null)
                        .OrderByDescending(x => x.Data)
                        .Skip((paginaAtual - 1) * itensPagina)
                        .Take(itensPagina)
                        .Fetch(x => x.Usuario)
                        .ThenFetch(x => x.Perfil)
                        .ToList();
        }
    }
}