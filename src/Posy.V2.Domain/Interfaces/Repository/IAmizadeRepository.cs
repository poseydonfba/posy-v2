using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IAmizadeRepository : IRepository<Amizade, Guid>
    {
        Amizade Get(Guid usuarioId1, Guid usuarioId2);
        Amizade GetSolicitacao(Guid usuarioIdSolicitante, Guid usuarioIdSolicitado);
        IEnumerable<Amizade> Get(Guid usuarioId, int paginaAtual, int itensPagina, SolicitacaoAmizade solicitacaoAmizade, out int totalRecords);
        IEnumerable<Usuario> GetAmigos(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
