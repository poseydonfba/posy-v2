using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IMembroService : IDisposable
    {
        Membro SolicitarParticipacao(int comunidadeId);
        void ExcluirMembro(int comunidadeId, int usuarioMembroId);
        void AdicionarMembro(int comunidadeId, int usuarioMembroId);
        void AceitarMembro(int comunidadeId, int usuarioId);
        void RecusarMembro(int comunidadeId, int usuarioId);
        Membro Obter(int comunidadeId, int usuarioId);
        IEnumerable<Membro> ObterMembros(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Membro> ObterMembrosPendentes(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Membro> ObterComunidades(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
