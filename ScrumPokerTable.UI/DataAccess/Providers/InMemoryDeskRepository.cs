using System;
using System.Collections.Generic;
using System.Linq;
using ScrumPokerTable.UI.DataAccess.Entities;

namespace ScrumPokerTable.UI.DataAccess.Providers
{
    public class InMemoryDeskRepository : IDeskRepository
    {
        public InMemoryDeskRepository()
        {
            _desks = new List<DeskEntity>();
            _users = new List<DeskUserEntity>();

            CreateDesk("d1", Enumerable.Range(1, 15).Select(x => x.ToString()).ToArray());
        }

        public DeskEntity GetDesk(string deskName)
        {
            var desk = _desks.SingleOrDefault(x => x.Name == deskName);
            if (desk == null)
            {
                throw new RepositoryException(string.Format("Desk {0} not found exists", deskName));
            }
            return desk;
        }

        public DeskUserEntity[] GetDeskUsers(string deskName)
        {
            GetDesk(deskName);
            return _users.Where(x => x.DeskName == deskName).ToArray();
        }

        public DeskUserEntity GetDeskUser(string deskName, string userName)
        {
            var user = GetDeskUsers(deskName).SingleOrDefault(x => x.Name == userName);
            if (user == null)
            {
                throw new RepositoryException(string.Format("Desk {0} does not contains user {1}", deskName, userName));
            }
            return user;
        }

        public void CreateDesk(string deskName, string[] cards)
        {
            if (_desks.Any(x => x.Name == deskName))
            {
                throw new RepositoryException(string.Format("Desk {0} already exists", deskName));
            }
            _desks.Add(new DeskEntity
            {
                Name = deskName,
                Cards = cards,
                State = DeskState.Unknown,
                Timestamp = DateTime.UtcNow
            });
        }

        public void CreateDeskUser(string deskName, string userName)
        {
            if (_users.Any(x => x.DeskName == deskName && x.Name == userName))
            {
                throw new RepositoryException(string.Format("Desk {0} already contains user {1}", deskName, userName));
            }
            _users.Add(new DeskUserEntity
            {
                DeskName = deskName,
                Name = userName,
                Card = null
            });
        }

        public void SetDeskState(string deskName, DeskState newState)
        {
            var desk = GetDesk(deskName);
            desk.State = newState;
            desk.Timestamp = DateTime.UtcNow;
        }

        public void SetUserCard(string deskName, string userName, string card)
        {
            var desk = GetDesk(deskName);
            var user = GetDeskUser(deskName, userName);
            user.Card = card;
            desk.Timestamp = DateTime.UtcNow;
        }

        private readonly List<DeskEntity> _desks;
        private readonly List<DeskUserEntity> _users;
    }
}