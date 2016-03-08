using System.Web.Http;
using Microsoft.Practices.Unity;
using Owin;
using ScrumPokerTable.UI.IoC;

namespace ScrumPokerTable.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = UnityContainerFactory.Create();
            ConfigureWebApi(app, container);
        }

        private static void ConfigureWebApi(IAppBuilder app, IUnityContainer container)
        {
            var config = new HttpConfiguration();
            config.DependencyResolver = new WebApiDependencyResolver(container);
            config.Routes.MapHttpRoute(
                name: "WebApi",
                routeTemplate: "api/1.0/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional});
            
            app.UseWebApi(config);
        }
    }
}