using System.Threading.Tasks;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.Hubs
{
    public interface IDeskServer
    {
        Task JoinAsUser(string deskName, string userName);

        Task JoinAsMaster(string deskName);

        Task Leave(string deskName);

        void SetUserCard(string deskName, string userName, string card);

        void SetDeskState(string deskName, DeskState newState);
    }
}