using ScrumPokerTable.UI.DataAccess.Entities;

namespace ScrumPokerTable.UI.DataAccess
{
    public interface IDeskRepository
    {
        DeskEntity GetDesk(string deskName);
        DeskUserEntity[] GetDeskUsers(string deskName);
        DeskUserEntity GetDeskUser(string deskName, string userName);
        
        void CreateDesk(string deskName, string[] cards);
        void CreateDeskUser(string deskName, string userName);

        void SetDeskState(string deskName, DeskState newState);
        void SetUserCard(string deskName, string userName, string card);
    }
}