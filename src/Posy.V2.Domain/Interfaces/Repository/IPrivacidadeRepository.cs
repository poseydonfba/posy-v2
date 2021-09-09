using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IPrivacidadeRepository : IRepository<Privacidade, Guid>
    {
        Privacidade GetByUsuario(Guid usuarioId);
    }
}
