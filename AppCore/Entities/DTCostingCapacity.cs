using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Entities
{

    [Table("DTCostingCapacities")]
    public class DTCostingCapacity : AutoIncreaseIdEntity
    {
        public string ModeOfTransportCode { get; set; }

        /// <summary>
        ///     CapacityValue
        /// </summary>
        public decimal CapacityValue { get; set; }

        /// <summary>
        ///     Mapping UoMOfTransportCode
        /// </summary>
        public string UoMOfTransportCode { get; set; }

        public string CapacityKey { get; set; }
    }
}
