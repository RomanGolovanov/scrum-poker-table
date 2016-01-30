using ScrumPokerTable.UI.DataAccess.Entities;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.DataAccess.Providers
{
    public interface IDeskProvider
    {
        DeskEntity GetDesk(string deskName);
        DeskUserEntity[] GetDeskUsers(string deskName);
        DeskUserEntity GetDeskUser(string deskName, string userName);
        
        string CreateDesk(string[] cards);
        void DeleteDesk(string deskName);

        void JoinUser(string deskName, string userName);

        void SetDeskState(string deskName, DeskState newState);
        void SetUserCard(string deskName, string userName, string card);
    }
}