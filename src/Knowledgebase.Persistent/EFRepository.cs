using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Knowledgebase.UnitOfWork;

namespace Knowledgebase.Persistent
{
    public class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _set;
        public EFRepository(DbContext dbContext)
        {
            _set = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _set.AsQueryable();
        }

        public TEntity Find(Guid id)
        {
            return _set.Find(id);
        }

        public void Insert(TEntity entity)
        {
            _set.Add(entity);
        }

        public void BatchInsert(ICollection<TEntity> entities)
        {
            foreach (var i in entities)
                _set.Add(i);
        }

        public void Update(TEntity entity)
        {
            _set.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            _set.Remove(entity);
        }
    }
}
