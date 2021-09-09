using NHibernate.Linq;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PostPerfilRepository : Repository<PostPerfil, int>, IPostPerfilRepository
    {
        public PostPerfilRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<PostPerfil> Get(int usuarioId, int paginaAtual, int itensPagina)
        {
            var query = (from c in DbSet
                         from d in Session.Query<PostOculto>()
                             .Where(m => m.PostPerfilId == c.Id).DefaultIfEmpty()
                         where c.UsuarioId == usuarioId
                         && c.Der == null
                         && (d.StatusPost == StatusPostOculto.Visivel || d == null)
                         select c)
                .Concat(from a in Session.Query<Usuario>()
                        join b in Session.Query<Amizade>() on a.Id equals b.SolicitadoParaId
                        join c in DbSet on b.SolicitadoParaId equals c.UsuarioId
                        from d in Session.Query<PostOculto>()
                            .Where(m => m.PostPerfilId == c.Id).DefaultIfEmpty()
                        where b.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                        && b.SolicitadoPorId == usuarioId
                        && c.Der == null
                        && b.Der == null
                        && (d.StatusPost == StatusPostOculto.Visivel || d == null)
                        select c)
                .Concat(from a in Session.Query<Usuario>()
                        join b in Session.Query<Amizade>() on a.Id equals b.SolicitadoPorId
                        join c in DbSet on b.SolicitadoPorId equals c.UsuarioId
                        from d in Session.Query<PostOculto>()
                            .Where(m => m.PostPerfilId == c.Id).DefaultIfEmpty()
                        where b.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                        && b.SolicitadoParaId == usuarioId
                        && c.Der == null
                        && b.Der == null
                        && (d.StatusPost == StatusPostOculto.Visivel || d == null)
                        select c);

            query = query/*.AsNoTracking()*/
                .OrderByDescending(x => x.DataPost)
                .Skip((paginaAtual - 1) * itensPagina)
                .Take(itensPagina)
                .Fetch(x => x.Usuario)
                .ThenFetch(x => x.Perfil);

            return query.ToList();
        }
    }
}