using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IRecadoService : IDisposable
    {
        Recado EnviarRecado(int enviadoParaId, string recado);
        void ExcluirRecado(int recadoId);
        void MarcarComoLido(int recadoId);
        Recado ObterRecado(int recadoId);
        IEnumerable<Recado> ObterRecadosEnviadosERecebidos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Recado> ObterRecadosRecebidosNaoLidos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
