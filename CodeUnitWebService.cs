using AdminAccount;
using JobRequisition;
using Microsoft.Extensions.Options;
using RPFBE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE
{
    public class CodeUnitWebService : ICodeUnitWebService
    {
        private readonly IOptions<WebserviceCreds> config;

        public CodeUnitWebService(IOptions<WebserviceCreds> config)
        {
            this.config = config;
        }
        //JRWS_PortClient jRWS;
        public JRWS_PortClient Client()
        {
            JRWS_PortClient jRWS = new JRWS_PortClient(JRWS_PortClient.EndpointConfiguration.JRWS_Port);
            jRWS.ClientCredentials.Windows.ClientCredential.UserName = config.Value.Username;
            jRWS.ClientCredentials.Windows.ClientCredential.Password = config.Value.Password;
            return jRWS;
        }
        public EmployeeAccountWebService_PortClient EmployeeAccount()
        {
            EmployeeAccountWebService_PortClient employeeAccountWebService = new EmployeeAccountWebService_PortClient(EmployeeAccountWebService_PortClient.EndpointConfiguration.EmployeeAccountWebService_Port);
            //employeeAccountWebService.ClientCredentials.Windows.ClientCredential.UserName = "MARVIN";
            //employeeAccountWebService.ClientCredentials.Windows.ClientCredential.Password = "husl2f5yqw";
            return employeeAccountWebService;
        }
    }
}
