using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IMembroService : IDisposable
    {
        Membro SolicitarParticipacao(Guid comunidadeId);
        void ExcluirMembro(Guid comunidadeId, Guid usuarioMembroId);
        void AdicionarMembro(Guid comunidadeId, Guid usuarioMembroId);
        void AceitarMembro(Guid comunidadeId, Guid usuarioId);
        void RecusarMembro(Guid comunidadeId, Guid usuarioId);
        Membro Obter(Guid comunidadeId, Guid usuarioId);
        IEnumerable<Membro> ObterMembros(Guid comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Membro> ObterMembrosPendentes(Guid comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Membro> ObterComunidades(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
