using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IMembroRepository : IRepository<Membro, Guid>
    {
        Membro Get(Guid comunidadeId, Guid usuarioId);
        IEnumerable<Membro> GetMembros(Guid comunidadeId, int paginaAtual, int itensPagina, StatusSolicitacaoMembroComunidade statusSolicitacaoMembroComunidade, out int totalRecords);
        IEnumerable<Membro> GetComunidades(Guid usuarioId, int paginaAtual, int itensPagina, StatusSolicitacaoMembroComunidade statusSolicitacaoMembroComunidade, out int totalRecords);
    }
}
