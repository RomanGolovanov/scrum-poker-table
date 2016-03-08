using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ScrumPokerTable.UI.Model;
using ScrumPokerTable.UI.Providers;
using ScrumPokerTable.UI.Providers.Exceptions;

namespace ScrumPokerTable.UI.Controllers
{
    public class DeskController : ApiController
    {
        public DeskController(IDeskProvider deskProvider)
        {
            _deskProvider = deskProvider;
        }

        public async Task<Desk> Get(string id)
        {
            try
            {
                var desk = _deskProvider.GetDesk(id);
                var pollingTimeout = GetPollingTimeout();
                var pollingTimestamp = GetPollingTimestamp();

                if (pollingTimeout==null || pollingTimestamp == null || desk.Timestamp != pollingTimestamp)
                {
                    return desk;
                }

                var start = DateTime.UtcNow;
                while (DateTime.UtcNow.Subtract(start) < pollingTimeout)
                {
                    await Task.Delay(100);
                    desk = _deskProvider.GetDesk(id);
                    if (desk.Timestamp != pollingTimestamp)
                    {
                        return desk;
                    }
                }

                throw new HttpResponseException(HttpStatusCode.NotModified);
            }
            catch (DeskNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
        


        public string Post([FromBody]string[] cards)
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

        private DateTimeOffset? GetPollingTimestamp()
        {
            IEnumerable<string> values;
            return Request.Headers.TryGetValues("X-Timestamp", out values)
                ? values.Select(DateTimeOffset.Parse).First()
                : (DateTimeOffset?)null;
        }

        private TimeSpan? GetPollingTimeout()
        {
            IEnumerable<string> values;
            return Request.Headers.TryGetValues("X-Polling-Timeout", out values)
                ? values.Select(x => TimeSpan.FromMilliseconds(int.Parse(x))).First()
                : (TimeSpan?)null;
        }

        private readonly IDeskProvider _deskProvider;
    }
}