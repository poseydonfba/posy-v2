using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class VisitantePerfilRepository : Repository<VisitantePerfil, Guid>, IVisitantePerfilRepository
    {
        public VisitantePerfilRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Perfil> GetVisitados(Guid usuarioId)
        {
            return _db.VisitantesPerfil.AsNoTracking()
                .Include("Visitado.Perfil")
                .Where(x => x.VisitanteId == usuarioId)
                .OrderByDescending(x => x.DataVisita)
                .Select(y => y.Visitante.Perfil)
                .Distinct()
                .ToList();
        }

        public IEnumerable<Perfil> GetVisitados(Guid usuarioId, int take)
        {
            return _db.VisitantesPerfil.AsNoTracking()
                .Include("Visitado.Perfil")
                .Where(x => x.VisitanteId == usuarioId)
                .OrderByDescending(x => x.DataVisita)
                .Select(y => y.Visitante.Perfil)
                .Distinct()
                .Take(take)
                .ToList();
        }

        public IEnumerable<Perfil> GetVisitantes(Guid usuarioId)
        {
            return _db.VisitantesPerfil.AsNoTracking()
                .Include("Visitante.Perfil")
                .Where(x => x.VisitadoId == usuarioId)
                .OrderByDescending(x => x.DataVisita)
                .Select(y => y.Visitante.Perfil)
                .Distinct()
                .ToList();
        }

        public IEnumerable<Perfil> GetVisitantes(Guid usuarioId, int take)
        {
            return _db.VisitantesPerfil.AsNoTracking()
                .Include("Visitante.Perfil")
                .Where(x => x.VisitadoId == usuarioId)
                .OrderByDescending(x => x.DataVisita)
                .Select(y => y.Visitante.Perfil)
                .Distinct()
                .Take(take)
                .ToList();
        }
    }
}