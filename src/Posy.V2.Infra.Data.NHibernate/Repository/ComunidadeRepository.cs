using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class ComunidadeRepository : Repository<Comunidade, int>, IComunidadeRepository
    {
        public ComunidadeRepository(DatabaseContext context) : base(context)
        {

        }

        public Comunidade GetById(int comunidadeId)
        {
            return _db.Session.Query<Comunidade>()
                .Fetch(x => x.Categoria)
                .Fetch(x => x.Usuario)
                .ThenFetch(x => x.Perfil)
                .Where(x => x.Id == comunidadeId)
                .FirstOrDefault();
        }

        public Comunidade GetByAlias(string alias)
        {
            return _db.Session.Query<Comunidade>()
                .Fetch(x => x.Categoria)
                .Fetch(x => x.Usuario)
                .ThenFetch(x => x.Perfil)
                .Where(x => x.Alias.ToLower() == alias.ToLower())
                .FirstOrDefault();
        }

        public IEnumerable<Comunidade> Get(int paginaAtual, int itensPagina, out int totalRecords)
        {
            totalRecords = _db.Session.Query<Comunidade>()/*.AsNoTracking()*/.Count();

            return _db.Session.Query<Comunidade>()/*.AsNoTracking()*/
                .Fetch(x => x.Usuario)
                .ThenFetch(x => x.Perfil)
                .OrderByDescending(x => x.Dir)
                .Skip((paginaAtual - 1) * itensPagina)
                .Take(itensPagina)
                .ToList();
        }

        public TopicoPost GetUltimoPost(int comunidadeId)
        {
            var query = (from c in _db.Session.Query<Topico>()
                         from d in _db.Session.Query<TopicoPost>()
                             .Where(m => m.TopicoId == c.Id).DefaultIfEmpty()
                         where c.ComunidadeId == comunidadeId
                         && c.Der == null
                         select d)
                         //.AsNoTracking()
                         .OrderByDescending(x => x.DataPost)
                         .FirstOrDefault();

            return query;
        }
    }
}