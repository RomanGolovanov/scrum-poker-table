using System.Linq;
using System.Web.Http;
using ScrumPokerTable.UI.DataAccess.Providers;
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
            var desk = _deskProvider.GetDesk(id);
            return new Desk
            {
                Name = desk.Name,
                Cards = desk.Cards,
                State = desk.State,
                Timestamp = desk.Timestamp,
                Users = _deskProvider.GetDeskUsers(id).Select(x => new DeskUser
                {
                    Name = x.Name,
                    Card = x.Card
                }).ToArray()
            };
        }

        private readonly IDeskProvider _deskProvider;
    }
}