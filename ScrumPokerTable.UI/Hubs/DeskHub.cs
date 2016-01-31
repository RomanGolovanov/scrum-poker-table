using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using ScrumPokerTable.UI.DataAccess;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.Hubs
{
    public class DeskHub : Hub<IDeskClient>, IDeskServer
    {
        public DeskHub(IDeskProvider deskProvider)
        {
            _deskProvider = deskProvider;
        }

        public async Task JoinAsUser(string deskName, string userName)
        {
            EnsureDeskExists(deskName);
            await Groups.Add(Context.ConnectionId, deskName);
            _deskProvider.JoinUser(deskName, userName);
            Clients.Group(deskName).DeskChanged(_deskProvider.GetDesk(deskName));
        }

        public async Task JoinAsMaster(string deskName)
        {
            EnsureDeskExists(deskName);
            await Groups.Add(Context.ConnectionId, deskName);
            Clients.Group(deskName).DeskChanged(_deskProvider.GetDesk(deskName));
        }

        public async Task Leave(string deskName)
        {
            EnsureDeskExists(deskName);
            await Groups.Remove(Context.ConnectionId, deskName);
            Clients.Group(deskName).DeskChanged(_deskProvider.GetDesk(deskName));
        }

        public void SetUserCard(string deskName, string userName, string card)
        {
            _deskProvider.SetUserCard(deskName, userName, card);
            Clients.Group(deskName).DeskChanged(_deskProvider.GetDesk(deskName));
        }

        public void SetDeskState(string deskName, DeskState newState)
        {
            _deskProvider.SetDeskState(deskName, newState);
            Clients.Group(deskName).DeskChanged(_deskProvider.GetDesk(deskName));
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