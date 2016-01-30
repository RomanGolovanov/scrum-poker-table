using System.Threading.Tasks;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.Hubs
{
    public interface IDeskServer
    {
        string CreateDesk(string[] cards);

        void DeleteDesk(string deskName);


        Task JoinAsUser(string deskName, string userName);

        Task JoinAsMaster(string deskName);

        Task Leave(string deskName);


        Desk GetDesk(string deskName);

        void SetUserCard(string deskName, string userName, string card);

        void SetDeskState(string deskName, DeskState newState);
        
    }
}