using AppCore.Contracts.SpecificationBase;
using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.RepositoryBase
{
    public interface IGenericRepository<TEntity> where TEntity : AutoIncreaseIdEntity
    {
        public Task<TEntity?> FindById(long id);
        public Task<TEntity?> SearchEntity(string keyWord);
        public Task<List<TEntity>> FindAsync(ISpecification<TEntity> spec);
        public Task<TEntity?> FindOneAsync(ISpecification<TEntity> spec);
        public Task<TEntity> AddAsync(TEntity entity);
        public Task<bool> AddBatchAsync(IEnumerable<TEntity> entities);
        public Task<bool> AddOrUpdateAsync(ISpecification<TEntity> spec, TEntity newEntity);
        public Task<bool> UpdateAsync(IEnumerable<TEntity> entity);
        public Task<bool> UpdateOneAsync(TEntity entity);
        public Task RemoveAsync(TEntity entity);

        public ValueTask<long> CountAsync(ISpecification<TEntity> spec);
    }
}
