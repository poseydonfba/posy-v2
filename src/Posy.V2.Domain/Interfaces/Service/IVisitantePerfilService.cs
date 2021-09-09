using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Service
{
    public interface IVisitantePerfilService : IDisposable
    {
        void SalvarVisita(Guid usuarioIdVisitado);
        IEnumerable<Perfil> ObterVisitantes(int take);
    }
}
