using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Linq;

namespace Posy.V2.Infra.Data.Repository
{
    public class DepoimentoRepository : Repository<Depoimento, int>, IDepoimentoRepository
    {
        public DepoimentoRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Depoimento> GetEnviados(int usuarioId, int paginaAtual, int itensPagina, StatusDepoimento flag, out int totalRecords)
        {
            var depoimentosEnviados = DbSet/*.AsNoTracking()*/
                .Fetch(x => x.EnviadoPor)
                .ThenFetch(x => x.Perfil)
                .Fetch(x => x.EnviadoPara)
                .ThenFetch(x => x.Perfil)
                .Where(DepoimentosEnviados(usuarioId, flag));

            totalRecords = depoimentosEnviados.Count();
            var depoimentos = depoimentosEnviados.OrderByDescending(x => x.DataDepoimento).Skip((paginaAtual - 1) * itensPagina).Take(itensPagina).ToList();

            return depoimentos;
        }

        public IEnumerable<Depoimento> GetRecebidos(int usuarioId, int paginaAtual, int itensPagina, StatusDepoimento flag, out int totalRecords)
        {
            var depoimentosRecebidos = DbSet/*.AsNoTracking()*/
                .Fetch(x => x.EnviadoPor)
                .ThenFetch(x => x.Perfil)
                .Fetch(x => x.EnviadoPara)
                .ThenFetch(x => x.Perfil)
                .Where(DepoimentosRecebidos(usuarioId, flag));

            totalRecords = depoimentosRecebidos.Count();
            var depoimentos = depoimentosRecebidos.OrderByDescending(x => x.DataDepoimento).Skip((paginaAtual - 1) * itensPagina).Take(itensPagina).ToList();

            return depoimentos;
        }

        public IEnumerable<Depoimento> GetEnviadosERecebidos(int usuarioId, int paginaAtual, int itensPagina, StatusDepoimento flag, out int totalRecords)
        {
            var depoimentosEnviados = DbSet/*.AsNoTracking()*/
                .Fetch(x => x.EnviadoPor)
                .ThenFetch(x => x.Perfil)
                .Fetch(x => x.EnviadoPara)
                .ThenFetch(x => x.Perfil)
                .Where(DepoimentosEnviados(usuarioId, flag));

            var depoimentosRecebidos = DbSet/*.AsNoTracking()*/
                .Fetch(x => x.EnviadoPor)
                .ThenFetch(x => x.Perfil)
                .Fetch(x => x.EnviadoPara)
                .ThenFetch(x => x.Perfil)
                .Where(DepoimentosRecebidos(usuarioId, flag));

            totalRecords = depoimentosRecebidos.Concat(depoimentosEnviados).Count();

            var depoimentos = depoimentosRecebidos.Concat(depoimentosEnviados)
                .OrderByDescending(x => x.DataDepoimento)
                .Skip((paginaAtual - 1) * itensPagina)
                .Take(itensPagina)
                .ToList();

            return depoimentos;
        }

        #region Expression

        private Expression<Func<Depoimento, bool>> DepoimentosEnviados(int usuarioId, StatusDepoimento flag)
        {
            if (flag == StatusDepoimento.Todos)
                return x => x.EnviadoPorId == usuarioId && x.Der == null && x.StatusDepoimento != StatusDepoimento.NaoAceito;
            else
                return x => x.EnviadoPorId == usuarioId && x.Der == null && x.StatusDepoimento == flag;
        }
        private Expression<Func<Depoimento, bool>> DepoimentosRecebidos(int usuarioId, StatusDepoimento flag)
        {
            if (flag == StatusDepoimento.Todos)
                return x => x.EnviadoParaId == usuarioId && x.Der == null && x.StatusDepoimento != StatusDepoimento.NaoAceito;
            else
                return x => x.EnviadoParaId == usuarioId && x.Der == null && x.StatusDepoimento == flag;
        }

        #endregion
    }
}