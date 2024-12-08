using IdentityProject.Utilities.EmailHandler.Models;

namespace IdentityProject.Utilities.EmailHandler.Abstract
{
    public interface IEmailService
    {
        void SendMessage(Message message);
    }
}
