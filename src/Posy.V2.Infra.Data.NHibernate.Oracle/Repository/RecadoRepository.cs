using NHibernate.Linq;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Posy.V2.Infra.Data.Repository
{
    public class RecadoRepository : Repository<Recado, int>, IRecadoRepository
    {
        public RecadoRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Recado> GetEnviados(int usuarioId, int paginaAtual, int itensPagina, StatusRecado statusRecado, out int totalRecords)
        {
            var recadosEnviados = DbSet
                .Fetch(x => x.EnviadoPor)
                .ThenFetch(x => x.Perfil)
                .Fetch(x => x.EnviadoPara)
                .ThenFetch(x => x.Perfil)
                .Where(RecadosEnviados(usuarioId, statusRecado));

            totalRecords = recadosEnviados.Count();
            var recados = recadosEnviados.OrderByDescending(x => x.DataRecado).Skip((paginaAtual - 1) * itensPagina).Take(itensPagina).ToList();

            return recados;
        }

        public IEnumerable<Recado> GetRecebidos(int usuarioId, int paginaAtual, int itensPagina, StatusRecado statusRecado, out int totalRecords)
        {
            var recadosRecebidos = DbSet
                .Fetch(x => x.EnviadoPor)
                .ThenFetch(x => x.Perfil)
                .Fetch(x => x.EnviadoPara)
                .ThenFetch(x => x.Perfil)
                .Where(RecadosRecebidos(usuarioId, statusRecado));

            totalRecords = recadosRecebidos.Count();
            var recados = recadosRecebidos.OrderByDescending(x => x.DataRecado).Skip((paginaAtual - 1) * itensPagina).Take(itensPagina).ToList();

            return recados;
        }

        public IEnumerable<Recado> GetEnviadosERecebidos(int usuarioId, int paginaAtual, int itensPagina, StatusRecado statusRecado, out int totalRecords)
        {
            var recadosEnviados = DbSet/* .AsNoTracking() */
                .Fetch(x => x.EnviadoPor)
                .ThenFetch(x => x.Perfil)
                .Fetch(x => x.EnviadoPara)
                .ThenFetch(x => x.Perfil)
                .Where(RecadosEnviados(usuarioId, statusRecado));

            var recadosRecebidos = DbSet/* .AsNoTracking() */
                .Fetch(x => x.EnviadoPor)
                .ThenFetch(x => x.Perfil)
                .Fetch(x => x.EnviadoPara)
                .ThenFetch(x => x.Perfil)
                .Where(RecadosRecebidos(usuarioId, statusRecado));

            totalRecords = recadosRecebidos.Concat(recadosEnviados).Count();

            var recados = recadosRecebidos.Concat(recadosEnviados)
                .OrderByDescending(x => x.DataRecado)
                .Skip((paginaAtual - 1) * itensPagina)
                .Take(itensPagina)
                .ToList();

            return recados;
        }

        #region Expression

        private Expression<Func<Recado, bool>> RecadosEnviados(int usuarioId, StatusRecado statusRecado)
        {
            if (statusRecado == StatusRecado.Todos)
                return x => x.EnviadoPorId == usuarioId && x.Der == null;
            else
                return x => x.EnviadoPorId == usuarioId && x.Der == null && x.StatusRecado == statusRecado;
        }
        private Expression<Func<Recado, bool>> RecadosRecebidos(int usuarioId, StatusRecado statusRecado)
        {
            if (statusRecado == StatusRecado.Todos)
                return x => x.EnviadoParaId == usuarioId && x.Der == null;
            else
                return x => x.EnviadoParaId == usuarioId && x.Der == null && x.StatusRecado == statusRecado;
        }

        #endregion
    }
}