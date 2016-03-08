using System.Web.Http;
using ScrumPokerTable.UI.Model;
using ScrumPokerTable.UI.Providers;

namespace ScrumPokerTable.UI.Controllers
{
    public class PlayerController : ApiController
    {
        public PlayerController(IDeskProvider deskProvider)
        {
            _deskProvider = deskProvider;
        }

        public void Post(string id, [FromBody]DeskUser user)
        {
            _deskProvider.JoinUser(id, user.Name);
        }

        public void Put(string id, [FromBody]DeskUser user)
        {
            _deskProvider.SetUserCard(id, user.Name, user.Card);
        }

        private readonly IDeskProvider _deskProvider;
    }
}