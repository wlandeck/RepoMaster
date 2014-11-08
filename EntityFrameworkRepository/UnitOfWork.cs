using EntityFrameworkRepository.Infrastructure;
using EntityFrameworkRepository.Interfaces;
using IRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkRepository
{
    public abstract class UnitOfWork<TContextType> : IUnitOfWork<TContextType> where TContextType : class
    {
        private readonly IDataContext _context;

        private bool _disposed;
        private Hashtable _repositories;

        public UnitOfWork(IDataContext context)
        {
            Guard.ArgumentNotNull(context, "DB Unit of Work Context");
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();

            _disposed = true;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
    }
}
