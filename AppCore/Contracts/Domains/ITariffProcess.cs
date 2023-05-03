using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.Domains
{
    public interface ITariffProcess
    {
        //public IReadOnlyList<ProcessFlow> ProcessFlows { get; }

        //public Inspector CurrentInspector { get; }

        //public IReadOnlyList<DTTariffMainItem> Items { get; }

        public string Status { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public bool IsPublish { get; set; }

        public string? ApprovalBy { get; set; }
        public DateTime? ApprovalAt { get; set; }


        public string? SubmitBy { get; set; }
        public DateTime? SubmitAt { get; set; }


        public string? DeclineBy { get; set; }
        public int? DeclineStep { get; set; }


        public int Step { get; set; }
        //public void ProcessStep(IWorkflowProcess workflowProcess);
        //public void InitProcessFlow(List<ProcessFlow> processFlows);
    }
}
