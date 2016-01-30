using ScrumPokerTable.UI.DataAccess.Entities;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.DataAccess
{
    public interface IDeskProvider
    {
        DeskEntity GetDesk(string deskName);
        DeskUserEntity[] GetDeskUsers(string deskName);
        DeskUserEntity GetDeskUser(string deskName, string userName);
        
        void CreateDesk(string deskName, string[] cards);
        void DeleteDesk(string deskName);

        void JoinUser(string deskName, string userName);

        void SetDeskState(string deskName, DeskState newState);
        void SetUserCard(string deskName, string userName, string card);
    }
}