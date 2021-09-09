using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Posy.V2.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity, TKey> : IDisposable where TEntity : class
    {
        void SaveOrUpdate(TEntity entity);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void Remove(TKey id);
        void RemovePermanente(TEntity entity);
        void RemovePermanente(TKey id);

        TEntity Get(TKey id);
        TEntity GetNoTracking(TKey id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllNoTracking();

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                           params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
