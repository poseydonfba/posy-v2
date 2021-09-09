using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Posy.V2.Infra.Data.Repository
{
    public class DepoimentoRepository : Repository<Depoimento, Guid>, IDepoimentoRepository
    {
        public DepoimentoRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Depoimento> GetEnviados(Guid usuarioId, int paginaAtual, int itensPagina, StatusDepoimento flag, out int totalRecords)
        {
            var depoimentosEnviados = _db.Depoimentos.AsNoTracking()
                .Include("EnviadoPor")
                .Include("EnviadoPor.Perfil")
                .Include("EnviadoPara")
                .Include("EnviadoPara.Perfil")
                .Where(DepoimentosEnviados(usuarioId, flag));

            totalRecords = depoimentosEnviados.Count();
            var depoimentos = depoimentosEnviados.OrderByDescending(x => x.DataDepoimento).Skip((paginaAtual - 1) * itensPagina).Take(itensPagina).ToList();

            return depoimentos;
        }

        public IEnumerable<Depoimento> GetRecebidos(Guid usuarioId, int paginaAtual, int itensPagina, StatusDepoimento flag, out int totalRecords)
        {
            var depoimentosRecebidos = _db.Depoimentos.AsNoTracking()
                .Include("EnviadoPor")
                .Include("EnviadoPor.Perfil")
                .Include("EnviadoPara")
                .Include("EnviadoPara.Perfil")
                .Where(DepoimentosRecebidos(usuarioId, flag));

            totalRecords = depoimentosRecebidos.Count();
            var depoimentos = depoimentosRecebidos.OrderByDescending(x => x.DataDepoimento).Skip((paginaAtual - 1) * itensPagina).Take(itensPagina).ToList();

            return depoimentos;
        }

        public IEnumerable<Depoimento> GetEnviadosERecebidos(Guid usuarioId, int paginaAtual, int itensPagina, StatusDepoimento flag, out int totalRecords)
        {
            var depoimentosEnviados = _db.Depoimentos.AsNoTracking()
                .Include("EnviadoPor")
                .Include("EnviadoPor.Perfil")
                .Include("EnviadoPara")
                .Include("EnviadoPara.Perfil")
                .Where(DepoimentosEnviados(usuarioId, flag));

            var depoimentosRecebidos = _db.Depoimentos.AsNoTracking()
                .Include("EnviadoPor")
                .Include("EnviadoPor.Perfil")
                .Include("EnviadoPara")
                .Include("EnviadoPara.Perfil")
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

        private Expression<Func<Depoimento, bool>> DepoimentosEnviados(Guid usuarioId, StatusDepoimento flag)
        {
            if (flag == StatusDepoimento.Todos)
                return x => x.EnviadoPorId == usuarioId && x.Der == null && x.StatusDepoimento != StatusDepoimento.NaoAceito;
            else
                return x => x.EnviadoPorId == usuarioId && x.Der == null && x.StatusDepoimento == flag;
        }
        private Expression<Func<Depoimento, bool>> DepoimentosRecebidos(Guid usuarioId, StatusDepoimento flag)
        {
            if (flag == StatusDepoimento.Todos)
                return x => x.EnviadoParaId == usuarioId && x.Der == null && x.StatusDepoimento != StatusDepoimento.NaoAceito;
            else
                return x => x.EnviadoParaId == usuarioId && x.Der == null && x.StatusDepoimento == flag;
        }

        #endregion
    }
}