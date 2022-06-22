using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Model
{
    public class WebserviceCreds
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }

        public string CorsAllowedUrl { get; set; }
        public string ExcelHostUrl { get; set; }
        //Dimension Codes
        public string Station { get; set; }
        public string Section { get; set; }
        public string Department { get; set; }

        //HR Docs
        public string HRDocFilePath { get; set; }

    }
}
