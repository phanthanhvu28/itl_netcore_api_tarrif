using AppCore.Contracts.SpecificationBase;
using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Specifications
{
    public class DTCostingCapacityGetByCapacityKeySpec : SpecificationBase<DTCostingCapacity>
    {
        private readonly string _key;

        public DTCostingCapacityGetByCapacityKeySpec(string key)
        {
            _key = key;

            ApplyFilter(entity => entity.CapacityKey == _key);
        }
    }
}
