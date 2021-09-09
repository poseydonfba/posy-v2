using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IAmizadeService : IDisposable
    {
        void AdicionarSolicitacaoAmizade(int usuarioIdSolicitado);
        void ExcluirAmigo(int usuarioIdParaExcluir);
        void AceitarSolicitacaoAmizade(int usuarioIdAceitar);
        void RecusarSolicitacaoAmizade(int usuarioIdRecusar);
        Amizade Obter(int usuarioId1, int usuarioId2);
        Amizade ObterAmigo(int usuarioId, int usuarioIdAmigo);
        IEnumerable<Amizade> SolicitacoesRecebidasPendentes(int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Usuario> ObterAmigos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
