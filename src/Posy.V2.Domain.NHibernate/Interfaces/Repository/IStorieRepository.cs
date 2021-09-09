using Posy.V2.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IStorieRepository : IRepository<Storie, int>
    {
        IEnumerable<Storie> ObterStories(int usuarioId);
    }
}
