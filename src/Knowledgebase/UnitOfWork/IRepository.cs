using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.Entities;

namespace Knowledgebase.UnitOfWork
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity Find(Guid id);

        void Insert(TEntity entity);
        void BatchInsert(ICollection<TEntity> entities);

        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
