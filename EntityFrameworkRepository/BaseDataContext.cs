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

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public new DbEntityEntry<T> Entry<T>(T entity) where T : class
        {
            return base.Entry<T>(entity);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public abstract void AddModelBuilderConfiguration(DbModelBuilder modelBuilder);

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            AddModelBuilderConfiguration(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public IEnumerable<T> SqlCommand<T>(string cmd) where T : class
        {
            return this.Database.SqlQuery<T>(cmd).ToList();
        }
    }
}
