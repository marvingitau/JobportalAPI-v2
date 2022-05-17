using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Model.DBEntity
{
    public class EmployeeClearance
    {
        public int Id { get; set; }
        public string UID { get; set; }
        public string ClearanceNo { get; set; }
        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public DateTime LastEmployeeDate { get; set; }
        public int ProgressFlag { get; set; } = 1;
        //The point the progress start ie 1 for HOD and 5 for FIN Admin
        public int ProgressStartFlag { get; set; } = 1;
       // [NotMapped]
        public string SelectedRole { get; set; }

        //HOD-HR Fields
        public string Year { get; set; }
        public float AnnualLeaveDays { get; set; }
        public float AnnualDaysLess { get; set; }
        public float BalDays { get; set; }

        //HOD-FIN Fields
        public float StaffLoan { get; set; }
        public float OtherLoan { get; set; }
        public float JitSavings { get; set; }
        public string AccountantOne { get; set; }
        public string NameOne { get; set; }
        public string AccountantTwo { get; set; }
        public string NameTwo { get; set; }
        public string FinanceManager { get; set; }
        public string FinanceManagerName { get; set; }
        public string FinanceDirector { get; set; }
        public string FinanceDirectorName { get; set; }


    }
}
