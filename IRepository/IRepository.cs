using BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity FindById(object id);
        IEnumerable<TEntity> SqlCommand(string command);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Insert(TEntity entity);
        IRepositoryQuery<TEntity> Query();
    }
}
