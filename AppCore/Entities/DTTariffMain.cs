using AppCore.Contracts.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Entities
{
    [Table("DTTariffMains")]
    public class DTTariffMain : AutoIncreaseIdEntity, ITariffProcess
    {
        
        public int? DeclineStep { get; set; }
        public int Step { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        /// <summary>
        ///     See Tariff.Domain.Constants.Tariff.Status
        /// </summary>
        public string Status { get; set; } = Constants.Tariff.Status.Temp;
        public string? ApprovalBy { get; set; }
        public DateTime? ApprovalAt { get; set; }
        public string? SubmitBy { get; set; }
        public DateTime? SubmitAt { get; set; }
        public string? DeclineBy { get; set; }
    }
}
