using System.Web.Http;
using ScrumPokerTable.UI.DataAccess;
using ScrumPokerTable.UI.Model;

namespace ScrumPokerTable.UI.Controllers
{
    public class DeskController : ApiController
    {
        public DeskController(IDeskProvider deskProvider)
        {
            _deskProvider = deskProvider;
        }

        public Desk Get(string id)
        {
            return _deskProvider.GetDesk(id);
        }

        private readonly IDeskProvider _deskProvider;
    }
}