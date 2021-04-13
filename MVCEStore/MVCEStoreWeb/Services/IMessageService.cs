using System.Threading.Tasks;

namespace MVCEStoreWeb.Services
{
    public interface IMessageService
    {
        Task SendEmailConfirmationMessage(string to, string name, string link);
    }
}
