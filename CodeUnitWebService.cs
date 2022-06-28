using AdminAccount;
using ApprovalMGT;
using DOCMgt;
using HRActivity;
using JobRequisition;
using Mailer;
using Microsoft.Extensions.Options;
using PayrollMGT;
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

            employeeAccountWebService.ClientCredentials.UserName.UserName = config.Value.Username;
            employeeAccountWebService.ClientCredentials.UserName.Password = config.Value.Password;

            return employeeAccountWebService;
        }

        public HRManagementWS_PortClient HRWS()
        {
            HRManagementWS_PortClient hRManagementWS = new HRManagementWS_PortClient(HRManagementWS_PortClient.EndpointConfiguration.HRManagementWS_Port);
            hRManagementWS.ClientCredentials.UserName.UserName = config.Value.Username;
            hRManagementWS.ClientCredentials.UserName.Password = config.Value.Password;

            return hRManagementWS;
        }

        public Notifications_PortClient WSMailer()
        {
            Notifications_PortClient notifications = new Notifications_PortClient(Notifications_PortClient.EndpointConfiguration.Notifications_Port);
            notifications.ClientCredentials.UserName.UserName = config.Value.Username;
            notifications.ClientCredentials.UserName.Password = config.Value.Password;

            return notifications;
        }

        public DocumentMgmt_PortClient DOCMGT()
        {
            DocumentMgmt_PortClient docMgt = new DocumentMgmt_PortClient(DocumentMgmt_PortClient.EndpointConfiguration.DocumentMgmt_Port);
            docMgt.ClientCredentials.UserName.UserName = config.Value.Username;
            docMgt.ClientCredentials.UserName.Password = config.Value.Password;

            return docMgt;
        }

        public PortalApprovalManager_PortClient ApprovalMGT()
        {
            PortalApprovalManager_PortClient appMgt = new PortalApprovalManager_PortClient(PortalApprovalManager_PortClient.EndpointConfiguration.PortalApprovalManager_Port);
            appMgt.ClientCredentials.UserName.UserName = config.Value.Username;
            appMgt.ClientCredentials.UserName.Password = config.Value.Password;

            return appMgt;
        }

        public PayrollManagementWebService_PortClient PayrollMGT()
        {
            PayrollManagementWebService_PortClient payMgt = new PayrollManagementWebService_PortClient(PayrollManagementWebService_PortClient.EndpointConfiguration.PayrollManagementWebService_Port);
            payMgt.ClientCredentials.UserName.UserName = config.Value.Username;
            payMgt.ClientCredentials.UserName.Password = config.Value.Password;

            return payMgt;
        }
    }
}
