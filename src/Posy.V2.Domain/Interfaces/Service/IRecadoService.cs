using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IRecadoService : IDisposable
    {
        Recado EnviarRecado(Guid enviadoParaId, string recado);
        void ExcluirRecado(Guid recadoId);
        void MarcarComoLido(Guid recadoId);
        Recado ObterRecado(Guid recadoId);
        IEnumerable<Recado> ObterRecadosEnviadosERecebidos(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Recado> ObterRecadosRecebidosNaoLidos(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
