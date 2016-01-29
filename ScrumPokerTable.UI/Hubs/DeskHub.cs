using System.Linq;
using Microsoft.AspNet.SignalR;
using ScrumPokerTable.UI.DataAccess;

namespace ScrumPokerTable.UI.Hubs
{
    public class DeskHub : Hub<IDeskClient>
    {
        public DeskHub(IDeskRepository deskRepository)
        {
            _deskRepository = deskRepository;
        }

        public void CreateDesk(string deskName)
        {
            _deskRepository.CreateDesk(deskName, Enumerable.Range(1,15).Select(x=>x.ToString()).ToArray());
        }

        public void ConnectDesk(string deskName, string userName)
        {
            var users = _deskRepository.GetDeskUsers(deskName);
            if (users.All(x => x.Name != userName))
            {
                _deskRepository.CreateDeskUser(deskName, userName);
                Groups.Add(Context.ConnectionId, deskName);
            }
            Clients.Group(deskName).DeskChanged(userName, "connected, users: " + string.Join(",", _deskRepository.GetDeskUsers(deskName).Select(x => x.Name)));
        }

        public void UpdateDeskUser(string deskName, string userName, string message)
        {
            _deskRepository.SetUserCard(deskName, userName, message);
            var userStates = _deskRepository.GetDeskUsers(deskName).Select(x => string.Format("{0}:{1}", x.Name, x.Card ?? "null"));
            Clients.Group(deskName).DeskChanged(userName, string.Join(",", userStates));
        }

        private readonly IDeskRepository _deskRepository;
    }
}