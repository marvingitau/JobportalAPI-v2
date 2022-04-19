using Microsoft.AspNetCore.Identity;
using RecruitmentPortalBE.Model.DBEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentPortalBE.Auth
{
    public class ApplicationUser:IdentityUser
    {
        [DefaultValue(0)]
        public int ProfileId { get; set; } = 0;
        public string EmployeeId { get; set; }
        public string Name { get; set; }
    }
}
