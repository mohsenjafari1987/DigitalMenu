using DigitalMenu.Model.Base;
using DigitalMenu.Repository.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMenu.Repository.GenericRepository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly IMongoDBContext _mongoContext;
        protected IMongoCollection<TEntity> _dbCollection;

        protected BaseRepository(IMongoDBContext context)
        {
            _mongoContext = context;
            _dbCollection = _mongoContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }


        public async Task<TEntity> Get(ObjectId id)
        {
            //FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", id);
            var data = await _dbCollection.FindAsync(r => r.Id == id);
            return data?.FirstOrDefault();

        }


        public async Task<IEnumerable<TEntity>> Get()
        {
            var data = await _dbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
            return data?.ToList();
        }

        public async Task Create(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }
            await _dbCollection.InsertOneAsync(obj);
        }

        public async Task Create(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }
            await _dbCollection.InsertManyAsync(entities);
        }

        public async Task Update(TEntity obj)
        {
            await _dbCollection.ReplaceOneAsync(r => r.Id == obj.GetId(), obj);
        }

        public async Task Delete(ObjectId id)
        {
            await _dbCollection.DeleteOneAsync(r => r.Id == id);
        }
    }
}
