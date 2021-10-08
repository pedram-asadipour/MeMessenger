using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Get()
        {
            return _dbSet.AsNoTracking();
        }

        public T GetBy(object entity)
        {
            return _dbSet.Find(entity);
        }

        public T GetBy(Expression<Func<T, bool>> where)
        {
            return _dbSet.SingleOrDefault(where);
        }

        public bool Exists(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Any(expression);
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Create(IEnumerable<T> entities)
        {
            if (entities == null)
                return;

            _dbSet.AddRange(entities);
        }

        public void Edit(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Edit(IEnumerable<T> entities)
        {
            if (entities == null)
                return;

            _dbSet.UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                return;

            _dbSet.RemoveRange(entities);
        }

        public void Delete(object entity)
        {
            var entityObject = GetBy(entity);
            Delete(entityObject);
        }
    }
}