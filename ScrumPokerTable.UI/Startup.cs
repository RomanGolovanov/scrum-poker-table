using System.Web.Http;
using Owin;

namespace ScrumPokerTable.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "WebApi", 
                routeTemplate: "api/1.0/{controller}",
                defaults: new {id = RouteParameter.Optional});

            
            app.UseWebApi(config);
            app.MapSignalR();
        }
    }
}