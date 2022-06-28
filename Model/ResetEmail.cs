using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Model
{
    public class ResetEmail
    {
        public string EmployeeId { get; set; }

        //Honey pot
        public bool Approved { get; set; }
    }
}
