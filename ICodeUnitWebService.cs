using AdminAccount;
using JobRequisition;

namespace RPFBE
{
    public interface ICodeUnitWebService
    {
        JWS_PortClient Client();
        EmployeeAccountWebService_PortClient EmployeeAccount();
    }
}