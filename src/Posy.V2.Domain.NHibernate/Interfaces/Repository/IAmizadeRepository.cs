using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IAmizadeRepository : IRepository<Amizade, int>
    {
        Amizade Get(int usuarioId1, int usuarioId2);
        Amizade GetSolicitacao(int usuarioIdSolicitante, int usuarioIdSolicitado);
        IEnumerable<Amizade> Get(int usuarioId, int paginaAtual, int itensPagina, SolicitacaoAmizade solicitacaoAmizade, out int totalRecords);
        IEnumerable<Usuario> GetAmigos(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
