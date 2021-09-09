using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IAmizadeService : IDisposable
    {
        void AdicionarSolicitacaoAmizade(Guid usuarioIdSolicitado);
        void ExcluirAmigo(Guid usuarioIdParaExcluir);
        void AceitarSolicitacaoAmizade(Guid usuarioIdAceitar);
        void RecusarSolicitacaoAmizade(Guid usuarioIdRecusar);
        Amizade Obter(Guid usuarioId1, Guid usuarioId2);
        Amizade ObterAmigo(Guid usuarioId, Guid usuarioIdAmigo);
        IEnumerable<Amizade> SolicitacoesRecebidasPendentes(int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Usuario> ObterAmigos(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
