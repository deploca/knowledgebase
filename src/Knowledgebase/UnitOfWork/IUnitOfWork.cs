using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Knowledgebase.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
