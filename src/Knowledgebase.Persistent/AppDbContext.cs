﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Knowledgebase.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Knowledgebase.Persistent
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        #region Unit Of Work

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new EFRepository<TEntity>(this);
        }

        void IUnitOfWork.SaveChanges()
        {
            this.SaveChanges();
        }

        Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return this.SaveChangesAsync(cancellationToken);
        }

        void IUnitOfWork.BeginTransaction()
        {
            if (!this.Database.IsInMemory())
                this.Database.BeginTransaction();
        }

        void IUnitOfWork.CommitTransaction()
        {
            if (!this.Database.IsInMemory())
                this.Database.CommitTransaction();
        }

        void IUnitOfWork.RollbackTransaction()
        {
            if (!this.Database.IsInMemory())
                this.Database.RollbackTransaction();
        }

        #endregion

        #region Entities

        public virtual DbSet<Entities.AppUser> Users { get; set; }
        public virtual DbSet<Entities.AppSetting> AppSettings { get; set; }
        public virtual DbSet<Entities.Category> Categories { get; set; }
        public virtual DbSet<Entities.Thread> Threads { get; set; }
        public virtual DbSet<Entities.Tag> Tags { get; set; }

        #endregion
    }
}
