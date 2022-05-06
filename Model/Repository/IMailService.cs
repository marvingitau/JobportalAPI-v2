using System.Threading.Tasks;
using WebAPITest.Models;

namespace RPFBE.Model.Repository
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendShortlistAsync(Shortlisted request);
        Task SendEmailPasswordReset(string userEmail, string link);
    }
}