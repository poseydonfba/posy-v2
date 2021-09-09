using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IStorieService : IDisposable
    {
        IEnumerable<Storie> ObterStories(int usuarioId, int paginaAtual, int itensPagina);
    }
}
