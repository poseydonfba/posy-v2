using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Commit(GlobalUser globalUser);
    }
}
