using AppCore.Contracts.SpecificationBase;
using AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Specifications
{
    public class DTCosingSearchMainSpec : SpecificationBase<DTCostingMain>
    {
        private readonly string _key;

        public DTCosingSearchMainSpec(string key)
        {
            _key = key;

            ApplyFilter(entity => entity.SupplierId == _key);
        }
    }
}
