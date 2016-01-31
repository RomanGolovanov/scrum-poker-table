using ScrumPokerTable.UI.Providers.Storage.Entities;

namespace ScrumPokerTable.UI.Providers.Storage
{
    public interface IDeskStorage
    {
        bool IsDeskExists(string deskName);

        DeskEntity GetDesk(string deskName);

        void CreateDesk(DeskEntity desk);
        void UpdateDesk(DeskEntity desk);
        void DeleteDesk(DeskEntity desk);
    }
}