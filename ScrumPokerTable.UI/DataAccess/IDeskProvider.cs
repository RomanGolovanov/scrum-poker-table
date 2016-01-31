using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.DataAccess
{
    public interface IDeskProvider
    {
        string CreateDesk(string[] cards);
        void DeleteDesk(string deskName);
        Desk GetDesk(string deskName);

        void JoinUser(string deskName, string userName);

        void SetDeskState(string deskName, DeskState newState);
        void SetUserCard(string deskName, string userName, string card);
    }
}