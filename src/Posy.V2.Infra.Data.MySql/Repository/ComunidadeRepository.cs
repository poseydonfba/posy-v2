using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class ComunidadeRepository : Repository<Comunidade, Guid>, IComunidadeRepository
    {
        public ComunidadeRepository(DatabaseContext context) : base(context)
        {

        }

        public Comunidade GetById(Guid comunidadeId)
        {
            return _db.Comunidades
                .Include("Categoria")
                .Include("Usuario")
                .Include("Usuario.Perfil")
                .Where(x => x.ComunidadeId == comunidadeId)
                .FirstOrDefault();
        }

        public Comunidade GetByAlias(string alias)
        {
            return _db.Comunidades
                .Include("Categoria")
                .Include("Usuario")
                .Include("Usuario.Perfil")
                .Where(x => x.Alias.ToLower() == alias.ToLower())
                .FirstOrDefault();
        }

        public IEnumerable<Comunidade> Get(int paginaAtual, int itensPagina, out int totalRecords)
        {
            totalRecords = _db.Comunidades.AsNoTracking().Count();

            return _db.Comunidades.AsNoTracking()
                .Include("Usuario")
                .Include("Usuario.Perfil")
                .OrderByDescending(x => x.Dir)
                .Skip((paginaAtual - 1) * itensPagina)
                .Take(itensPagina)
                .ToList();
        }

        public TopicoPost GetUltimoPost(Guid comunidadeId)
        {
            var query = (from c in _db.Topicos
                         from d in _db.TopicosPosts
                             .Where(m => m.TopicoId == c.TopicoId).DefaultIfEmpty()
                         where c.ComunidadeId == comunidadeId
                         && c.Der == null
                         select d)
                         .AsNoTracking()
                         .OrderByDescending(x => x.DataPost)
                         .FirstOrDefault();

            return query;
        }
    }
}