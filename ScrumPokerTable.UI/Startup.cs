﻿using System.Web.Http;
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
            config.Routes.MapHttpRoute(
                name: "WebApi",
                routeTemplate: "api/1.0/{controller}",
                defaults: new {id = RouteParameter.Optional});
            
            app.UseWebApi(config);
        }

        private static void ConfigureSignalR(IAppBuilder app, IUnityContainer container)
        {
            var hubConfiguration = new HubConfiguration
            {
                Resolver = new SignalrDependencyResolver(container)
            };
            app.MapSignalR(hubConfiguration);
        }
    }
}