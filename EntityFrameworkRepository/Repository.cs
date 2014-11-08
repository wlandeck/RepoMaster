using EntityFrameworkRepository.Infrastructure;
using EntityFrameworkRepository.Interfaces;
using IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkRepository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal IDataContext Context;
        internal IDbSet<TEntity> DbSet;

        public Repository(IDataContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual TEntity FindById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Update(TEntity entity)
        {
            Guard.ArgumentNotNull(entity, "Update Entity");
            DbSet.Attach(entity);
            DbEntityEntry<TEntity> entry = Context.Entry<TEntity>(entity);
            entry.State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            var entity = DbSet.Find(id);
            Delete(entity);
        }

        public IEnumerable<TEntity> SqlCommand(string command)
        {
            return Context.SqlCommand<TEntity>(command);
        }

        public virtual void Delete(TEntity entity)
        {
            Guard.ArgumentNotNull(entity, "Delete Entity");
            DbSet.Attach(entity);
            DbSet.Remove(entity);
        }

        public virtual void Insert(TEntity entity)
        {
            Guard.ArgumentNotNull(entity, "Insert Entity");
            DbSet.Add(entity);

        }

        public virtual IRepositoryQuery<TEntity> Query()
        {
            var repositoryGetFluentHelper =
                new RepositoryQuery<TEntity>(this);

            return repositoryGetFluentHelper;
        }

        internal IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>>
                includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query.ToList();
        }
    }

}
