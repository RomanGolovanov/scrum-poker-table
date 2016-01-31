using System;
using System.Collections.Generic;
using System.Linq;
using ScrumPokerTable.UI.DataAccess.Entities;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.DataAccess
{
    public class InMemoryDeskProvider : IDeskProvider
    {
        public InMemoryDeskProvider(IDeskNameProvider deskNameProvider)
        {
            _deskNameProvider = deskNameProvider;
            _desks = new List<DeskEntity>();
            _users = new List<DeskUserEntity>();
        }

        public string CreateDesk(string[] cards)
        {
            while (true)
            {
                var deskName = _deskNameProvider.GetNewDeskName();
                if(_desks.Any(x=>x.Name == deskName))
                    continue;

                _desks.Add(new DeskEntity
                {
                    Name = deskName,
                    Cards = cards,
                    State = DeskState.Voting,
                    Timestamp = DateTime.UtcNow
                });

                return deskName;
            }
        }

        public void DeleteDesk(string deskName)
        {
            _desks.Remove(GetDeskEntity(deskName));
        }

        public void JoinUser(string deskName, string userName)
        {
            if (_users.Any(x => x.DeskName == deskName && x.Name == userName))
            {
                return;
            }
            _users.Add(new DeskUserEntity
            {
                DeskName = deskName,
                Name = userName,
                Card = null
            });
        }

        public Desk GetDesk(string deskName)
        {
            var desk = GetDeskEntity(deskName);
            return new Desk
            {
                Name = desk.Name,
                Cards = desk.Cards,
                State = desk.State,
                Timestamp = desk.Timestamp,
                Users = GetDeskUsers(deskName).Select(x => new DeskUser
                {
                    Name = x.Name,
                    Card = x.Card
                }).ToArray()
            };
        }

        public void SetDeskState(string deskName, DeskState newState)
        {
            var desk = GetDeskEntity(deskName);
            if (desk.State == newState)
            {
                throw new DeskLogicException(string.Format("Desk {0} already has the state {1}", deskName, newState));
            }
            desk.State = newState;
            desk.Timestamp = DateTime.UtcNow;
        }

        public void SetUserCard(string deskName, string userName, string card)
        {
            var desk = GetDeskEntity(deskName);
            if (!desk.Cards.Contains(card))
            {
                throw new DeskLogicException(string.Format("Card {0} is not supported by desk {1}", card, deskName));
            }

            if (desk.State != DeskState.Voting)
            {
                throw new DeskLogicException(string.Format("Cannot change user {0} card to {1} while desk {2} not in Voting state", userName, card, deskName));
            }

            var user = GetDeskUser(deskName, userName);
            user.Card = card;
            if (GetDeskUsers(deskName).All(x=>x.Card!=null) && desk.State == DeskState.Voting)
            {
                desk.State = DeskState.Display;
            }
            desk.Timestamp = DateTime.UtcNow;

        }

        public DeskEntity GetDeskEntity(string deskName)
        {
            var desk = _desks.SingleOrDefault(x => x.Name == deskName);
            if (desk == null)
            {
                throw new DeskLogicException(string.Format("Desk {0} not found exists", deskName));
            }
            return desk;
        }

        private DeskUserEntity[] GetDeskUsers(string deskName)
        {
            return _users.Where(x => x.DeskName == deskName).ToArray();
        }

        private DeskUserEntity GetDeskUser(string deskName, string userName)
        {
            var user = GetDeskUsers(deskName).SingleOrDefault(x => x.Name == userName);
            if (user == null)
            {
                throw new DeskLogicException(string.Format("Desk {0} does not contains user {1}", deskName, userName));
            }
            return user;
        }


        private readonly IDeskNameProvider _deskNameProvider;

        private readonly List<DeskEntity> _desks;
        private readonly List<DeskUserEntity> _users;
    }
}