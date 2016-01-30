using System.Diagnostics;
using System.Reflection;
using System.Web.Http;

namespace ScrumPokerTable.UI.Controllers
{
    public class VersionController : ApiController
    {
        public string Get()
        {
            return FileVersionInfo
                .GetVersionInfo(Assembly.GetExecutingAssembly().Location)
                .ProductVersion;
        }
    }
}