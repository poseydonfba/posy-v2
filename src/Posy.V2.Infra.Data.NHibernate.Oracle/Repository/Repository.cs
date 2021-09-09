using NHibernate;
using NHibernate.Linq;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Interfaces.Repository;
using Posy.V2.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Posy.V2.Infra.Data.Repository
{
    public class Repository<TEntity, TKey> : IDisposable, IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly DatabaseContext _db;
        protected readonly IQueryable<TEntity> DbSet;
        protected ISession Session; //{ get { return _db.Session; } }

        public Repository(DatabaseContext context)
        {
            _db = context;
            Session = _db.Session;// _db.Set<TEntity>();
            DbSet = Session.Query<TEntity>();
        }

        public virtual void SaveOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public virtual void Insert(TEntity entity)
        {
            Session.Save(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Session.Update(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            Session.Update(entity);
        }

        public virtual void Remove(TKey id)
        {
            Session.Update(Session.Load<TEntity>(id));
        }

        public virtual void RemovePermanente(TEntity entity)
        {
            Session.Delete(entity);
        }

        public virtual void RemovePermanente(TKey id)
        {
            Session.Delete(Session.Load<TEntity>(id));
        }

        public virtual TEntity Get(TKey id)
        {
            return Session.Get<TEntity>(id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public TEntity GetNoTracking(TKey id)
        {
            var entity = Session.Load<TEntity>(id);
            //_db.Entry(entity).State = EntityState.Detached;
            return entity;

            //DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id); // EF Core
        }

        public IQueryable<TEntity> GetAllNoTracking()
        {
            return DbSet/*.AsNoTracking()*/;
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
                query = query.Fetch(includeProperty);
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
