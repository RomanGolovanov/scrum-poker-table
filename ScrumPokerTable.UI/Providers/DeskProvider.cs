using System;
using System.Linq;
using ScrumPokerTable.UI.Model;
using ScrumPokerTable.UI.Providers.Exceptions;
using ScrumPokerTable.UI.Providers.Naming;
using ScrumPokerTable.UI.Providers.Storage;
using ScrumPokerTable.UI.Providers.Storage.Entities;

namespace ScrumPokerTable.UI.Providers
{
    public class DeskProvider : IDeskProvider
    {
        public DeskProvider(IDeskNameProvider deskNameProvider, IDeskStorage deskStorage)
        {
            _deskNameProvider = deskNameProvider;
            _deskStorage = deskStorage;
        }

        #region Desk Management

        public string CreateDesk(string[] cards)
        {
            while (true)
            {
                var deskName = _deskNameProvider.GetNewDeskName();
                if(_deskStorage.IsDeskExists(deskName))
                    continue;

                _deskStorage.CreateDesk(new DeskEntity
                {
                    Name = deskName,
                    Cards = cards,
                    State = DeskState.Voting,
                    Users = new DeskUserEntity[0],
                    Timestamp = DateTime.UtcNow
                });

                return deskName;
            }
        }

        public void DeleteDesk(string deskName)
        {
            _deskStorage.DeleteDesk(_deskStorage.GetDesk(deskName));
        }

        public Desk GetDesk(string deskName)
        {
            var desk = _deskStorage.GetDesk(deskName);
            return new Desk
            {
                Name = desk.Name,
                Cards = desk.Cards,
                State = desk.State,
                Timestamp = desk.Timestamp,
                Users = desk.Users.Select(x => new DeskUser
                {
                    Name = x.Name,
                    Card = x.Card
                }).ToArray()
            };
        }

        public void SetDeskState(string deskName, DeskState newState)
        {
            var desk = _deskStorage.GetDesk(deskName);
            if (desk.State == newState)
            {
                throw new DeskLogicException(string.Format("Desk {0} already has the state {1}", deskName, newState));
            }

            if (newState == DeskState.Voting)
            {
                foreach (var user in desk.Users)
                {
                    user.Card = null;
                }
            }

            desk.State = newState;
            desk.Timestamp = DateTime.UtcNow;
            _deskStorage.UpdateDesk(desk);
        }

        #endregion

        #region User Management

        public void JoinUser(string deskName, string userName)
        {
            var desk = _deskStorage.GetDesk(deskName);
            if (desk.Users.Any(x => x.Name.Equals(userName, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }
            var newUser = new DeskUserEntity { Name = userName, Card = null };

            desk.Users = desk.Users.Concat(new [] { newUser }).ToArray();
            desk.Timestamp = DateTime.UtcNow;
            _deskStorage.UpdateDesk(desk);
        }

        public void SetUserCard(string deskName, string userName, string card)
        {
            var desk = _deskStorage.GetDesk(deskName);
            if (!desk.Cards.Contains(card))
            {
                throw new DeskLogicException(string.Format("Card {0} is not supported by desk {1}", card, deskName));
            }

            if (desk.State != DeskState.Voting)
            {
                throw new DeskLogicException(string.Format("Cannot change user {0} card to {1} while desk {2} not in Voting state", userName, card, deskName));
            }

            var user = desk.GetDeskUser(userName);
            user.Card = card;
            if (desk.Users.All(x=>x.Card!=null) && desk.State == DeskState.Voting)
            {
                desk.State = DeskState.Display;
            }
            desk.Timestamp = DateTime.UtcNow;
            _deskStorage.UpdateDesk(desk);
        }

        #endregion

        private readonly IDeskNameProvider _deskNameProvider;
        private readonly IDeskStorage _deskStorage;
    }
}