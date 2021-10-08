using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> Get();

        public T GetBy(object entity);

        public T GetBy(Expression<Func<T, bool>> where);

        public bool Exists(Expression<Func<T, bool>> expression);

        public void Create(T entity);

        public void Create(IEnumerable<T> entities);

        public void Edit(T entity);

        public void Edit(IEnumerable<T> entities);

        public void Delete(T entity);

        public void Delete(IEnumerable<T> entities);
        public void Delete(object entity);
    }
}