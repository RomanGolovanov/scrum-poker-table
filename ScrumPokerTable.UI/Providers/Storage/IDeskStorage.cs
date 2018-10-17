using ScrumPokerTable.UI.Providers.Storage.Entities;
using System.Collections.Generic;

namespace ScrumPokerTable.UI.Providers.Storage
{
    public interface IDeskStorage
    {
        bool IsDeskExists(string deskName);

        DeskEntity GetDesk(string deskName);

        IReadOnlyCollection<DeskEntity> GetDesks();

        void CreateDesk(DeskEntity desk);
        void UpdateDesk(DeskEntity desk);
        void DeleteDesk(DeskEntity desk);
    }
}