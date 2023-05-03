using AppCore.Contracts.SpecificationBase;
using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.RepositoryBase
{
    public interface IDTCostingMainRepository : IGenericRepository<DTCostingMain>
    {
        Task<bool> UpdateCapacity(ISpecification<DTCostingMain> spec, decimal itemCapacityValue);
    }
}
