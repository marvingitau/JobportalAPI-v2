using AdminAccount;
using JobRequisition;
using Mailer;

namespace RPFBE
{
    public interface ICodeUnitWebService
    {
        JWS_PortClient Client();
        EmployeeAccountWebService_PortClient EmployeeAccount();
        Notifications_PortClient WSMailer();
    }
}