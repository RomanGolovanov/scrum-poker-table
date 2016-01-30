using System.Threading.Tasks;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.Hubs
{
    public interface IDeskServer
    {
        Task JoinAsUser(string deskName, string userName);

        Task JoinAsMaster(string deskName);

        void CreateDesk(string deskName, string[] cards);

        void DeleteDesk(string deskName);

        void SetUserCard(string deskName, string userName, string card);

        void SetDeskState(string deskName, DeskState newState);
        
    }
}