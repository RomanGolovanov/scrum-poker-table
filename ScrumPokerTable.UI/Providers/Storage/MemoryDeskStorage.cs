using System.Collections.Generic;
using System.Linq;
using ScrumPokerTable.UI.Providers.Exceptions;
using ScrumPokerTable.UI.Providers.Storage.Entities;

namespace ScrumPokerTable.UI.Providers.Storage
{
    public class MemoryDeskStorage : IDeskStorage
    {
        public bool IsDeskExists(string deskName)
        {
            return _desks.Any(x=>x.Name == deskName);
        }

        public DeskEntity GetDesk(string deskName)
        {
            return GetDeskEntity(deskName);
        }

        public void CreateDesk(DeskEntity desk)
        {
            if (IsDeskExists(desk.Name))
            {
                throw new DeskLogicException(string.Format("Desk {0} already exists", desk.Name));
            }
            _desks.Add(desk);
        }

        public void UpdateDesk(DeskEntity desk)
        {
            // do nothing
        }

        public void DeleteDesk(DeskEntity desk)
        {
            _desks.Remove(desk);
        }

        private DeskEntity GetDeskEntity(string deskName)
        {
            var desk = _desks.SingleOrDefault(x => x.Name == deskName);
            if (desk == null)
            {
                throw new DeskNotFoundException(string.Format("Desk {0} not found exists", deskName));
            }
            return desk;
        }

        private readonly List<DeskEntity> _desks = new List<DeskEntity>(); 
    }
}