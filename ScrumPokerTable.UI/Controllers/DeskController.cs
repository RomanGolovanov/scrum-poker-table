using System.Net;
using System.Web.Http;
using ScrumPokerTable.UI.DataAccess;
using ScrumPokerTable.UI.DataAccess.Exceptions;
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
            try
            {
                return _deskProvider.GetDesk(id);
            }
            catch (DeskNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public string Put([FromBody]string[] cards)
        {
            return _deskProvider.CreateDesk(cards);
        }

        public void Delete(string id)
        {
            try
            {
                _deskProvider.DeleteDesk(id);
            }
            catch (DeskNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        private readonly IDeskProvider _deskProvider;
    }
}