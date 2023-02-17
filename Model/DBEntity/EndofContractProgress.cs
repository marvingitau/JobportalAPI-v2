using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Model.DBEntity
{
    public class EndofContractProgress
    {
        public int Id { get; set; }
        public string UID { get; set; }//Manager
        public string UIDComment { get; set; }//Manager
        public string UIDTwo { get; set; }//HR
        public string UIDTwoComment { get; set; }//HR
        public string UIDThree { get; set; }//MD/FD
        public string UIDThreeComment { get; set; }//MD/FD
        public int ContractStatus { get; set; } = 0;
        public string ContractNo { get; set; }

        public string EmpID { get; set; }
        public string MgrID { get; set; }
        public string MgrName { get; set; }
        public string SupervisionTime { get; set; }
        public string DoRenew { get; set; }
        public string EmpName { get; set; }
        public string CreationDate { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string Position { get; set; }
        public string RenewReason { get; set; }
        public string Howlong { get; set; }


        public string BackTrackingReason { get; set; }

    }
}
