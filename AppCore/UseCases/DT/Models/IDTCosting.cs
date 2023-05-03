using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.UseCases.DT.Models
{
    public interface IDTCosting
    {
        public string[] CapacityFields { get; set; }
        public string GetCapacityKey();
    }
}
