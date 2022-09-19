using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Model
{
    public class MonitoringHeader
    {
        public string StaffID { get; set; }
        public string MonitorID { get; set; }
        //Non-Confirmation
        public DateTime ContractEndDate { get; set; }
        //Confirmation
        public DateTime ContractDate { get; set; }
        public DateTime ContractExpire { get; set; }
        //Extend
        public string ExtendDuration { get; set; }
        public DateTime ExtendDate { get; set; }
        public DateTime NextReviewDate { get; set; }

    }
}
