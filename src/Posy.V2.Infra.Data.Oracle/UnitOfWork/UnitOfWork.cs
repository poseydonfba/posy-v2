using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces;
using Posy.V2.Infra.Data.Context;
using System;

namespace Posy.V2.Infra.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ICurrentUser _currentUser;
        private bool _disposed = false;

        public UnitOfWork(DatabaseContext databaseContext, ICurrentUser currentUser)
        {
            _databaseContext = databaseContext;
            _currentUser = currentUser;
        }

        public void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            _databaseContext.SaveChanges(_currentUser.GetCurrentUser());
        }

        public void Commit(GlobalUser globalUser)
        {
            if (_disposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            _databaseContext.SaveChanges(globalUser);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing && _databaseContext != null)
                _databaseContext.Dispose();

            _disposed = true;
        }
    }
}
