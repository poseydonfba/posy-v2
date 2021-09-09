using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IMembroRepository : IRepository<Membro, int>
    {
        Membro Get(int comunidadeId, int usuarioId);
        IEnumerable<Membro> GetMembros(int comunidadeId, int paginaAtual, int itensPagina, StatusSolicitacaoMembroComunidade statusSolicitacaoMembroComunidade, out int totalRecords);
        IEnumerable<Membro> GetComunidades(int usuarioId, int paginaAtual, int itensPagina, StatusSolicitacaoMembroComunidade statusSolicitacaoMembroComunidade, out int totalRecords);
    }
}
