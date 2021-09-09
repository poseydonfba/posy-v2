using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IRecadoRepository : IRepository<Recado, Guid>
    {
        IEnumerable<Recado> GetEnviados(Guid usuarioId, int paginaAtual, int itensPagina, StatusRecado statusRecado, out int totalRecords);
        IEnumerable<Recado> GetRecebidos(Guid usuarioId, int paginaAtual, int itensPagina, StatusRecado statusRecado, out int totalRecords);
        IEnumerable<Recado> GetEnviadosERecebidos(Guid usuarioId, int paginaAtual, int itensPagina, StatusRecado statusRecado, out int totalRecords);
    }
}
