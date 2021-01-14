using DigitalMenu.Model.Base;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMenu.Repository.GenericRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        Task Create(TEntity obj);
        Task Create(IEnumerable<TEntity> entities);
        Task Update(TEntity obj);
        Task Delete(ObjectId id);
        Task<TEntity> Get(ObjectId id);
        Task<IEnumerable<TEntity>> Get();
    }

}
