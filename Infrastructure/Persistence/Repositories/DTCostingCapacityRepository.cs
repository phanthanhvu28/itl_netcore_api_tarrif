using AppCore.Contracts.RepositoryBase;
using AppCore.Contracts.SpecificationBase;
using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class DTCostingCapacityRepository : GenericRepositoryBase<DTCostingCapacity>, IDTCostingCapacityRepository
    {
        private readonly MainDbContext _dbContext;

        public DTCostingCapacityRepository(MainDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public override async Task<bool> AddOrUpdateAsync(ISpecification<DTCostingCapacity> spec, DTCostingCapacity @new)
        {
            DTCostingCapacity entity = await FindOneAsync(spec) ?? @new;
            _dbContext.Attach(entity);
            entity.CapacityValue = @new.CapacityValue;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
