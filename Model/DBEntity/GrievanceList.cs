using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Model.DBEntity
{
    public class GrievanceList
    {
        public int Id { get; set; }
        public string GID { get; set; }
        public string Employeeno { get; set; }
        public string Supervisor { get; set; }
        public string Currentstage { get; set; }
        public string Nextstage { get; set; }

        public string Employeename { get; set; }
        public string Supervisorname { get; set; }
        public string GrievanceType { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string StepTaken { get; set; }
        public string Outcome { get; set; }
        public string Comment { get; set; }
        public string Recommendation { get; set; }

        public bool Resolved { get; set; }
        public string Resolver { get; set; }
        public string ResolverID { get; set; }

        public int CycleNo { get; set; } = 0;
        public int ProgressNo { get; set; } = 0;
        public string NextStageStaff { get; set; }
        public string Action { get; set; }
        public string Actionuser { get; set; }
        public string Actiondetails { get; set; }

        public string CycletwoInitreason { get; set; }
        public string Cycletwosteps { get; set; }
        public string Cycletwooutcome { get; set; }
        public string Cycletworecommendation { get; set; }

        public string AppealAlternativeRemark { get; set; }
        public string AppealOutcomeRemark { get; set; }





    }
}
