using AdminAccount;
using JobRequisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentPortalBE
{
    public class CodeUnitWebService
    {
        //JRWS_PortClient jRWS;
        public static JRWS_PortClient Client()
        {
            JRWS_PortClient jRWS = new JRWS_PortClient(JRWS_PortClient.EndpointConfiguration.JRWS_Port);
            return jRWS;
        }
        public static EmployeeAccountWebService_PortClient EmployeeAccount()
        {
            EmployeeAccountWebService_PortClient employeeAccountWebService = new EmployeeAccountWebService_PortClient(EmployeeAccountWebService_PortClient.EndpointConfiguration.EmployeeAccountWebService_Port);
            //employeeAccountWebService.ClientCredentials.Windows.ClientCredential.UserName = "MARVIN";
            //employeeAccountWebService.ClientCredentials.Windows.ClientCredential.Password = "husl2f5yqw";
            return employeeAccountWebService;
        }
    }
}
