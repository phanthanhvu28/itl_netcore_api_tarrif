using AppCore.Contracts.Common;
using AppCore.Contracts.SpecificationBase;
using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.RepositoryBase
{
    public class GenericRepositoryBase<TEntity> : IGenericRepository<TEntity> where TEntity : AutoIncreaseIdEntity
    {
        private readonly AppDbContextBase _dbContext;

        public GenericRepositoryBase(AppDbContextBase dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> AddBatchAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> AddOrUpdateAsync(ISpecification<TEntity> spec, TEntity newEntity)
        {
            TEntity entity = await FindOneAsync(spec) ?? newEntity;
            _dbContext.Attach(entity);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public virtual async ValueTask<long> CountAsync(ISpecification<TEntity> spec)
        {
            spec.IsPagingEnabled = false;
            IQueryable<TEntity> specificationResult = GetQueryable(_dbContext.Set<TEntity>(), spec);

            return await ValueTask.FromResult(specificationResult.LongCount());
        }

        public virtual async Task<List<TEntity>> FindAsync(ISpecification<TEntity> spec)
        {
            return GetQueryable(_dbContext.Set<TEntity>(), spec).AsNoTracking().ToList();
        }

        public virtual async Task<TEntity?> FindById(long id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public Task<TEntity?> SearchEntity(string keyWord)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity?> FindOneAsync(ISpecification<TEntity> spec)
        {
            return GetQueryable(_dbContext.Set<TEntity>(), spec).AsNoTracking().FirstOrDefault();
        }

        public virtual async Task RemoveAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(IEnumerable<TEntity> entity)
        {
            _dbContext.UpdateRange(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateOneAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        protected static IQueryable<TEntity> GetQueryable(
        IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification)
        {
            IQueryable<TEntity> query = inputQuery;

            if (specification.Criterias is { Count: > 0 })
            {
                Expression<Func<TEntity, bool>> expr = specification.Criterias.First();
                for (int i = 1; i < specification.Criterias.Count; i++)
                {
                    expr = expr.And(specification.Criterias.ElementAt(i));
                }

                query = query.Where(expr);
            }

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            if (specification.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy is not null)
            {
                //TODO: got exception 'The LINQ expression could not be translated. Either rewrite the query in a form that can be translated, or switch to client evaluation' when use GroupBy
                //Temp convert to AsEnumerable to AsQueryable avoid exception
                query = query.GroupBy(specification.GroupBy)
                    .SelectMany(x => x);
            }

            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip - 1)
                    .Take(specification.Take);
            }

            return query;
        }

      
    }
}
