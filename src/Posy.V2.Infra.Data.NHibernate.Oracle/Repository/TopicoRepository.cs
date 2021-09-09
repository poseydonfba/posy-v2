using NHibernate.Linq;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class TopicoRepository : Repository<Topico, int>, ITopicoRepository
    {
        public TopicoRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Topico> GetTopicos(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            totalRecords = DbSet.Where(x => x.ComunidadeId == comunidadeId && x.Der == null).Count();

            var topicos = DbSet
                            .Fetch(x => x.Usuario.Perfil)
                            .Where(x => x.ComunidadeId == comunidadeId && x.Der == null)
                            .OrderByDescending(x => x.DataTopico)
                            .Skip((paginaAtual - 1) * itensPagina)
                            .Take(itensPagina)
                            .ToList();

            return topicos;
        }
    }
}