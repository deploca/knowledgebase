using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Knowledgebase.UnitOfWork
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        void Insert(TEntity entity);
        void BatchInsert(ICollection<TEntity> entities);

        void Update(TEntity entity);
    }
}
