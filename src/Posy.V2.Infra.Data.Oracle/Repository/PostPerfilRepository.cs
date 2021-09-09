using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class PostPerfilRepository : Repository<PostPerfil, Guid>, IPostPerfilRepository
    {
        public PostPerfilRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<PostPerfil> Get(Guid usuarioId, int paginaAtual, int itensPagina)
        {
            var query = (from c in _db.PostsPerfil
                         from d in _db.PostsOculto
                             .Where(m => m.PostPerfilId == c.PostPerfilId).DefaultIfEmpty()
                         where c.UsuarioId == usuarioId
                         && c.Der == null
                         && (d.StatusPost == StatusPostOculto.Visivel || d == null)
                         select c)
                .Concat(from a in _db.Usuarios
                        join b in _db.Amizades on a.UsuarioId equals b.SolicitadoParaId
                        join c in _db.PostsPerfil on b.SolicitadoParaId equals c.UsuarioId
                        from d in _db.PostsOculto
                            .Where(m => m.PostPerfilId == c.PostPerfilId).DefaultIfEmpty()
                        where b.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                        && b.SolicitadoPorId == usuarioId
                        && c.Der == null
                        && b.Der == null
                        && (d.StatusPost == StatusPostOculto.Visivel || d == null)
                        select c)
                .Concat(from a in _db.Usuarios
                        join b in _db.Amizades on a.UsuarioId equals b.SolicitadoPorId
                        join c in _db.PostsPerfil on b.SolicitadoPorId equals c.UsuarioId
                        from d in _db.PostsOculto
                            .Where(m => m.PostPerfilId == c.PostPerfilId).DefaultIfEmpty()
                        where b.StatusSolicitacao == SolicitacaoAmizade.Aprovado
                        && b.SolicitadoParaId == usuarioId
                        && c.Der == null
                        && b.Der == null
                        && (d.StatusPost == StatusPostOculto.Visivel || d == null)
                        select c);

            query = query.AsNoTracking()
                .OrderByDescending(x => x.DataPost)
                .Skip((paginaAtual - 1) * itensPagina)
                .Take(itensPagina)
                .Include("Usuario")
                .Include("Usuario.Perfil");

            return query.ToList();
        }
    }
}