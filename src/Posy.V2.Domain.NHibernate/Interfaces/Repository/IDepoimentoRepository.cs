using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IDepoimentoRepository : IRepository<Depoimento, int>
    {
        IEnumerable<Depoimento> GetEnviados(int usuarioId, int paginaAtual, int itensPagina, StatusDepoimento statusDepoimento, out int totalRecords);
        IEnumerable<Depoimento> GetRecebidos(int usuarioId, int paginaAtual, int itensPagina, StatusDepoimento statusDepoimento, out int totalRecords);
        IEnumerable<Depoimento> GetEnviadosERecebidos(int usuarioId, int paginaAtual, int itensPagina, StatusDepoimento statusDepoimento, out int totalRecords);
    }
}
