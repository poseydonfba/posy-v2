using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IModeradorService : IDisposable
    {
        Moderador AdicionarModerador(Guid comunidadeId, Guid usuarioModeradorId);
        void ExcluirModerador(Guid comunidadeId, Guid usuarioId);
        Moderador Obter(Guid comunidadeId, Guid usuarioId);
        IEnumerable<Moderador> ObterModeradores(Guid comunidadeId);
        IEnumerable<Moderador> ObterModeradores(Guid comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Moderador> ObterComunidades(Guid usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
