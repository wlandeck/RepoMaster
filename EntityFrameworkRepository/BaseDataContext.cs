using BaseEntity;
using EntityFrameworkRepository.Interfaces;
using IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkRepository
{
    public abstract class BaseDataContext : DbContext, IDataContext
    {
        protected BaseDataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<BaseDataContext>(null);
        }

        public new IDbSet<T> Set<T>() where T : Entity
        {
            return base.Set<T>();
        }

        public new DbEntityEntry<T> Entry<T>(T entity) where T : Entity
        {
            return base.Entry<T>(entity);
        }

        public override int SaveChanges()
        {
            this.ApplyAuditChanges();
            return base.SaveChanges();
        }

        public abstract void AddModelBuilderConfiguration(DbModelBuilder modelBuilder);

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            AddModelBuilderConfiguration(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public void ApplyAuditChanges()
        {
            foreach (var dbEntityEntry in this.ChangeTracker.Entries().Where(e => e.Entity is IAuditDateInfo && (e.State == EntityState.Modified || e.State == EntityState.Added)))
            {
                var e = (IAuditDateInfo)dbEntityEntry.Entity;
                if (dbEntityEntry.State == EntityState.Added)
                {
                    e.CreatedOn = DateTime.Now;
                }
                e.ModifiedOn = DateTime.Now;
            }
        }

        public IEnumerable<T> SqlCommand<T>(string cmd) where T : Entity
        {
            return this.Database.SqlQuery<T>(cmd).ToList();
        }
    }
}
