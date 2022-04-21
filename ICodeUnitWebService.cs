using AdminAccount;
using JobRequisition;

namespace RPFBE
{
    public interface ICodeUnitWebService
    {
        JRWS_PortClient Client();
        EmployeeAccountWebService_PortClient EmployeeAccount();
    }
}