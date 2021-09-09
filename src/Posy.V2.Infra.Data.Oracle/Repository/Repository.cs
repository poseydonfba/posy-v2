using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Posy.V2.Infra.Data.Repository
{
    public class Repository<TEntity, TKey> : IDisposable, IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly DatabaseContext _db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(DatabaseContext context)
        {
            _db = context;
            DbSet = _db.Set<TEntity>();
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(TKey id)
        {
            _db.Entry(DbSet.Find(id)).State = EntityState.Modified;
        }

        public virtual void RemovePermanente(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void RemovePermanente(TKey id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public virtual TEntity Get(TKey id)
        {
            return DbSet.Find(id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public TEntity GetNoTracking(TKey id)
        {
            var entity = DbSet.Find(id);
            _db.Entry(entity).State = EntityState.Detached;
            return entity;

            //DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id); // EF Core
        }

        public IQueryable<TEntity> GetAllNoTracking()
        {
            return DbSet.AsNoTracking();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
