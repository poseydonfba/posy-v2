using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IPrivacidadeRepository : IRepository<Privacidade, int>
    {
        Privacidade GetByUsuario(int id);
    }
}
