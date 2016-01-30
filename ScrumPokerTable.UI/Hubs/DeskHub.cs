using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using ScrumPokerTable.UI.DataAccess.Providers;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.Hubs
{
    public class DeskHub : Hub<IDeskClient>, IDeskServer
    {
        public DeskHub(IDeskProvider deskProvider)
        {
            _deskProvider = deskProvider;
        }

        public string CreateDesk(string[] cards)
        {
            return _deskProvider.CreateDesk(cards);
        }

        public void DeleteDesk(string deskName)
        {
            _deskProvider.DeleteDesk(deskName);
        }

        public async Task JoinAsUser(string deskName, string userName)
        {
            EnsureDeskExists(deskName);
            await Groups.Add(Context.ConnectionId, deskName);
            _deskProvider.JoinUser(deskName, userName);
            Clients.Group(deskName).DeskChanged(GetDesk(deskName));
        }

        public async Task JoinAsMaster(string deskName)
        {
            EnsureDeskExists(deskName);
            await Groups.Add(Context.ConnectionId, deskName);
            Clients.Group(deskName).DeskChanged(GetDesk(deskName));
        }

        public async Task Leave(string deskName)
        {
            EnsureDeskExists(deskName);
            await Groups.Remove(Context.ConnectionId, deskName);
            Clients.Group(deskName).DeskChanged(GetDesk(deskName));
        }

        public Desk GetDesk(string deskName)
        {
            var desk = _deskProvider.GetDesk(deskName);
            return new Desk
            {
                Name = desk.Name,
                Cards = desk.Cards,
                State = desk.State,
                Timestamp = desk.Timestamp,
                Users = _deskProvider.GetDeskUsers(deskName).Select(x => new DeskUser
                {
                    Name = x.Name,
                    Card = x.Card
                }).ToArray()
            };
        }

        public void SetUserCard(string deskName, string userName, string card)
        {
            _deskProvider.SetUserCard(deskName, userName, card);
            Clients.Group(deskName).DeskChanged(GetDesk(deskName));
        }

        public void SetDeskState(string deskName, DeskState newState)
        {
            _deskProvider.SetDeskState(deskName, newState);
            Clients.Group(deskName).DeskChanged(GetDesk(deskName));
        }

        #region Private methods

        private void EnsureDeskExists(string deskName)
        {
            _deskProvider.GetDesk(deskName);
        }

        #endregion

        private readonly IDeskProvider _deskProvider;
    }
}