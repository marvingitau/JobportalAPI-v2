﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Model
{
    public class RequisionModel
    {
        public string Requisiontype { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public string Contracttype { get; set; }
        public string Department { get; set; }
        public string Employeereplaced { get; set; }
        public string HOD { get; set; }
        public string HRManager { get; set; }
        public string MD { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public string RequestedNo { get; set; }
    }
}
