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
    public class RecadoRepository : Repository<Recado, Guid>, IRecadoRepository
    {
        public RecadoRepository(DatabaseContext context) : base(context)
        {

        }

        public IEnumerable<Recado> GetEnviados(Guid usuarioId, int paginaAtual, int itensPagina, StatusRecado statusRecado, out int totalRecords)
        {
            var recadosEnviados = _db.Recados
                .Include("EnviadoPor")
                .Include("EnviadoPor.Perfil")
                .Include("EnviadoPara")
                .Include("EnviadoPara.Perfil")
                .Where(RecadosEnviados(usuarioId, statusRecado));

            totalRecords = recadosEnviados.Count();
            var recados = recadosEnviados.OrderByDescending(x => x.DataRecado).Skip((paginaAtual - 1) * itensPagina).Take(itensPagina).ToList();

            return recados;
        }

        public IEnumerable<Recado> GetRecebidos(Guid usuarioId, int paginaAtual, int itensPagina, StatusRecado statusRecado, out int totalRecords)
        {
            var recadosRecebidos = _db.Recados
                .Include("EnviadoPor")
                .Include("EnviadoPor.Perfil")
                .Include("EnviadoPara")
                .Include("EnviadoPara.Perfil")
                .Where(RecadosRecebidos(usuarioId, statusRecado));

            totalRecords = recadosRecebidos.Count();
            var recados = recadosRecebidos.OrderByDescending(x => x.DataRecado).Skip((paginaAtual - 1) * itensPagina).Take(itensPagina).ToList();

            return recados;
        }

        public IEnumerable<Recado> GetEnviadosERecebidos(Guid usuarioId, int paginaAtual, int itensPagina, StatusRecado statusRecado, out int totalRecords)
        {
            var recadosEnviados = _db.Recados.AsNoTracking()
                .Include("EnviadoPor")
                .Include("EnviadoPor.Perfil")
                .Include("EnviadoPara")
                .Include("EnviadoPara.Perfil")
                .Where(RecadosEnviados(usuarioId, statusRecado));

            var recadosRecebidos = _db.Recados.AsNoTracking()
                .Include("EnviadoPor")
                .Include("EnviadoPor.Perfil")
                .Include("EnviadoPara")
                .Include("EnviadoPara.Perfil")
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

        private Expression<Func<Recado, bool>> RecadosEnviados(Guid usuarioId, StatusRecado statusRecado)
        {
            if (statusRecado == StatusRecado.Todos)
                return x => x.EnviadoPorId == usuarioId && x.Der == null;
            else
                return x => x.EnviadoPorId == usuarioId && x.Der == null && x.StatusRecado == statusRecado;
        }
        private Expression<Func<Recado, bool>> RecadosRecebidos(Guid usuarioId, StatusRecado statusRecado)
        {
            if (statusRecado == StatusRecado.Todos)
                return x => x.EnviadoParaId == usuarioId && x.Der == null;
            else
                return x => x.EnviadoParaId == usuarioId && x.Der == null && x.StatusRecado == statusRecado;
        }

        #endregion
    }
}