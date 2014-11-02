using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IUnitOfWork<TContextType> where TContextType : class
    {
        void Dispose();
        void Commit();
        void Dispose(bool disposing);
        IRepository<T> Repository<T>() where T : class;
    }
}
