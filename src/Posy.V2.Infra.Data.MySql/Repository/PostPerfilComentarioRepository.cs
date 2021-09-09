using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PostPerfilComentarioRepository : Repository<PostPerfilComentario, Guid>, IPostPerfilComentarioRepository
    {
        public PostPerfilComentarioRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<PostPerfilComentario> Get(Guid postId, int paginaAtual, int itensPagina)
        {
            if (itensPagina == 0)
                return _db.PostsPerfilComentario
                        .Where(x => x.PostPerfilId == postId && x.Der == null)
                        .OrderByDescending(x => x.Data)
                        .Include("Usuario")
                        .Include("Usuario.Perfil")
                        .ToList();
            else
                return _db.PostsPerfilComentario
                        .Where(x => x.PostPerfilId == postId && x.Der == null)
                        .OrderByDescending(x => x.Data)
                        .Skip((paginaAtual - 1) * itensPagina)
                        .Take(itensPagina)
                        .Include("Usuario")
                        .Include("Usuario.Perfil")
                        .ToList();
        }
    }
}