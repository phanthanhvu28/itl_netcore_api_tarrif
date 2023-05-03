using AppCore.Contracts.RepositoryBase;
using AppCore.Contracts.SpecificationBase;
using AppCore.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public class DTCostingMainRepository : GenericRepositoryBase<DTCostingMain>, IDTCostingMainRepository
    {
        private readonly MainDbContext _dbContext;

        public DTCostingMainRepository(MainDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UpdateCapacity(ISpecification<DTCostingMain> spec, decimal itemCapacityValue)
        {
            IEnumerable<DTCostingMain> result = await FindAsync(spec);
            foreach (DTCostingMain entity in result)
            {
                entity.Capacity = itemCapacityValue;
            }

            _dbContext.UpdateRange(result);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
