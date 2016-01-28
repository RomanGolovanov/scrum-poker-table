using System.Linq;
using Microsoft.AspNet.SignalR;

namespace ScrumPokerTable.UI.Hubs
{
    public class DeskHub : Hub<IDeskClient>
    {
        private static readonly DeskRepository DeskRepository = new DeskRepository();

        public void ConnectDesk(string deskName, string userName)
        {
            Groups.Add(Context.ConnectionId, deskName);

            var desk = DeskRepository.GetOrCreateDesk(deskName);
            desk.GetOrCreateUser(userName);

            Clients.Group(deskName).DeskChanged(userName, "connected, users: " + string.Join(",", desk.Users.Select(x=>x.Name)));
        }

        public void UpdateDeskUser(string deskName, string userName, string message)
        {
            Groups.Add(Context.ConnectionId, deskName);

            Clients.Group(deskName).DeskChanged(userName, message);
        }
    }
}