using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IDepoimentoService : IDisposable
    {
        Depoimento EnviarDepoimento(int enviadoParaId, string depoimento);
        void ExcluirDepoimento(int depoimentoId);
        void AceitarDepoimento(int depoimentoId);
        void RecusarDepoimento(int depoimentoId);
        Depoimento ObterDepoimento(int depoimentoId);
        IEnumerable<Depoimento> ObterDepoimentosEnviados(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Depoimento> ObterDepoimentosRecebidos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
