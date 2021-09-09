using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IModeradorService : IDisposable
    {
        Moderador AdicionarModerador(int comunidadeId, int usuarioModeradorId);
        void ExcluirModerador(int comunidadeId, int usuarioId);
        Moderador Obter(int comunidadeId, int usuarioId);
        IEnumerable<Moderador> ObterModeradores(int comunidadeId);
        IEnumerable<Moderador> ObterModeradores(int comunidadeId, int paginaAtual, int itensPagina, out int totalRecords);
        IEnumerable<Moderador> ObterComunidades(int usuarioId, int paginaAtual, int itensPagina, out int totalRecords);
    }
}
