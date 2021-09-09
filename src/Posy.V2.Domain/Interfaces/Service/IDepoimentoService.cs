using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IDepoimentoService : IDisposable
    {
        Depoimento EnviarDepoimento(Guid enviadoParaId, string depoimento);
        void ExcluirDepoimento(Guid depoimentoId);
        void AceitarDepoimento(Guid depoimentoId);
        void RecusarDepoimento(Guid depoimentoId);
        Depoimento ObterDepoimento(Guid depoimentoId);
        IEnumerable<Depoimento> ObterDepoimentosEnviados(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Depoimento> ObterDepoimentosRecebidos(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
