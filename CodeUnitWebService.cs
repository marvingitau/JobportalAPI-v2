﻿using AdminAccount;
using HRActivity;
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
        public JWS_PortClient Client()
        {
            JWS_PortClient jRWS = new JWS_PortClient(JWS_PortClient.EndpointConfiguration.JWS_Port);
            //jRWS.ClientCredentials.Windows.ClientCredential.UserName = config.Value.Username;
            //jRWS.ClientCredentials.Windows.ClientCredential.Password = config.Value.Password;
            jRWS.ClientCredentials.UserName.UserName = config.Value.Username;
            jRWS.ClientCredentials.UserName.Password = config.Value.Password;
            return jRWS;
        }
        public EmployeeAccountWebService_PortClient EmployeeAccount()
        {
            EmployeeAccountWebService_PortClient employeeAccountWebService = new EmployeeAccountWebService_PortClient(EmployeeAccountWebService_PortClient.EndpointConfiguration.EmployeeAccountWebService_Port);
            //employeeAccountWebService.ClientCredentials.Windows.ClientCredential.UserName = "MARVIN";
            //employeeAccountWebService.ClientCredentials.Windows.ClientCredential.Password = "husl2f5yqw";

           // employeeAccountWebService.ClientCredentials.UserName.UserName = config.Value.Username;
           // employeeAccountWebService.ClientCredentials.UserName.Password = config.Value.Password;

            return employeeAccountWebService;
        }

        public HRManagementWS_PortClient HRWS()
        {
            HRManagementWS_PortClient hRManagementWS = new HRManagementWS_PortClient(HRManagementWS_PortClient.EndpointConfiguration.HRManagementWS_Port);
            hRManagementWS.ClientCredentials.UserName.UserName = config.Value.Username;
            hRManagementWS.ClientCredentials.UserName.Password = config.Value.Password;

            return hRManagementWS;
        }
    }
}
