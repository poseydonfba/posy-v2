using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class TopicoRepository : Repository<Topico, Guid>, ITopicoRepository
    {
        public TopicoRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Topico> GetTopicos(Guid comunidadeId, int paginaAtual, int itensPagina, out int totalRecords)
        {
            totalRecords = _db.Topicos.Where(x => x.ComunidadeId == comunidadeId && x.Der == null).Count();

            var topicos = _db.Topicos
                            .Include("Usuario.Perfil")
                            .Where(x => x.ComunidadeId == comunidadeId && x.Der == null)
                            .OrderByDescending(x => x.DataTopico)
                            .Skip((paginaAtual - 1) * itensPagina)
                            .Take(itensPagina)
                            .ToList();

            return topicos;
        }
    }
}