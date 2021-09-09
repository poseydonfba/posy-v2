using NHibernate.Linq;
using Posy.V2.Domain.Comparer;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class VisitantePerfilRepository : Repository<VisitantePerfil, int>, IVisitantePerfilRepository
    {
        public VisitantePerfilRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Perfil> GetVisitados(int usuarioId)
        {
            return DbSet/*.AsNoTracking()*/
                .Fetch(x => x.Visitado)
                .ThenFetch(x => x.Perfil)
                .Where(x => x.VisitanteId == usuarioId)
                .OrderByDescending(x => x.DataVisita)
                .Select(y => y.Visitante.Perfil)
                .Distinct()
                .ToList();
        }

        public IEnumerable<Perfil> GetVisitados(int usuarioId, int take)
        {
            return DbSet/*.AsNoTracking()*/
                .Fetch(x => x.Visitado)
                .ThenFetch(x => x.Perfil)
                .Where(x => x.VisitanteId == usuarioId)
                .OrderByDescending(x => x.DataVisita)
                .Select(y => y.Visitante.Perfil)
                .Distinct()
                .Take(take)
                .ToList();
        }

        public IEnumerable<Perfil> GetVisitantes(int usuarioId)
        {
            return DbSet/*.AsNoTracking()*/
                .Fetch(x => x.Visitante)
                .ThenFetch(x => x.Perfil)
                .Where(x => x.VisitadoId == usuarioId)
                .OrderByDescending(x => x.DataVisita)
                .Select(y => y.Visitante.Perfil)
                .Distinct()
                .ToList();
        }

        public IEnumerable<Perfil> GetVisitantes(int usuarioId, int take)
        {
            // ESTA OCORRENDO ERRO PELO DISTINCT CAMPOS BLOB
            //return _db.VisitantesPerfil.AsNoTracking()
            //    .Include("Visitante.Perfil")
            //    .Where(x => x.VisitadoId == usuarioId)
            //    .OrderByDescending(x => x.DataVisita)
            //    .Select(y => y.Visitante.Perfil)
            //    .Distinct()
            //    .Take(take)
            //    .ToList();

            // TIVE QUE USAR .AsEnumerable() POIS SEM ELE DA O ERRO ABAIXO
            // LINQ to Entities does not recognize the method 'System.Linq.IQueryable`1[Posy.V2.Domain.Entities.Perfil] 
            // Distinct[Perfil](System.Linq.IQueryable`1[Posy.V2.Domain.Entities.Perfil], 
            // System.Collections.Generic.IEqualityComparer`1[Posy.V2.Domain.Entities.Perfil])' 
            // method, and this method cannot be translated into a store expression.

            // USANDO O COMPARADOR PERSONALIZADO PerfilComparer
            //List<Perfil> perfis = _db.VisitantesPerfil.AsNoTracking()
            //    .Include("Visitante.Perfil")
            //    .Where(x => x.VisitadoId == usuarioId)
            //    .OrderByDescending(x => x.DataVisita)
            //    .AsEnumerable()
            //    .Select(y => y.Visitante.Perfil)
            //    .Distinct(new PerfilComparer())
            //    .Take(take)
            //    .ToList();

            //return perfis;


            // USANDO O COMPARADOR GENERICO Posy.V2.Domain.Comparer.GenericEqualityComparer
            Func<Perfil, object> fieldComparator = perfil => perfil.Id;

            GenericEqualityComparer<Perfil> genericPerfilIEqualityComparer = new GenericEqualityComparer<Perfil>(fieldComparator);

            List<Perfil> perfis = DbSet/*.AsNoTracking()*/
                .Fetch(x => x.Visitante)
                .ThenFetch(x => x.Perfil)
                .Where(x => x.VisitadoId == usuarioId)
                .OrderByDescending(x => x.DataVisita)
                .AsEnumerable()
                .Select(y => y.Visitante.Perfil)
                .Distinct(genericPerfilIEqualityComparer)
                .Take(take)
                .ToList();

            return perfis;
        }
    }

    //public class PerfilComparer : IEqualityComparer<Perfil>
    //{
    //    #region IEqualityComparer<Perfil> Perfis

    //    public bool Equals(Perfil x, Perfil y)
    //    {
    //        return x.UsuarioId.Equals(y.UsuarioId);
    //    }

    //    public int GetHashCode(Perfil obj)
    //    {
    //        return obj.UsuarioId.GetHashCode();
    //    }

    //    #endregion
    //}

}