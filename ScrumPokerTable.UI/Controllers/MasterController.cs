using System.Web.Http;
using ScrumPokerTable.UI.Model;
using ScrumPokerTable.UI.Providers;

namespace ScrumPokerTable.UI.Controllers
{
    public class MasterController : ApiController
    {
        public MasterController(IDeskProvider deskProvider)
        {
            _deskProvider = deskProvider;
        }

        public void Post(string id, [FromBody]DeskState newState)
        {
            _deskProvider.SetDeskState(id, newState);
        }

        private readonly IDeskProvider _deskProvider;
    }
}