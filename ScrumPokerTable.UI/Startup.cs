using System;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
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
            ConfigureSignalR(app, container);
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

        private static void ConfigureSignalR(IAppBuilder app, IUnityContainer container)
        {
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(2);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(6);
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(2);

            var hubConfiguration = new HubConfiguration
            {
                Resolver = new SignalrDependencyResolver(container)
            };
            app.MapSignalR(hubConfiguration);
        }
    }
}