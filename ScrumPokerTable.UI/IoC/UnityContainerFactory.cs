using Microsoft.Practices.Unity;
using ScrumPokerTable.UI.Providers;
using ScrumPokerTable.UI.Providers.Naming;
using ScrumPokerTable.UI.Providers.Storage;
using ScrumPokerTable.UI.Providers.Storage.LiteDbStorage;
using ScrumPokerTable.UI.Settings;

namespace ScrumPokerTable.UI.IoC
{
    public static class UnityContainerFactory
    {
        public static IUnityContainer Create()
        {
            var settings = ScrumPockerSettingsReader.ReadSettings();
            var liteDbFilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/desk.db");

            var container = new UnityContainer()
                .RegisterType<IDeskNameProvider, DeskNameProvider>()
                .RegisterType<IDeskProvider, DeskProvider>()
                .RegisterType<IDeskStorageCleanupWorker, DeskStorageCleanupWorker>(
                    new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c =>
                        new DeskStorageCleanupWorker(
                            c.Resolve<IDeskStorage>(),
                            settings.DeskTTL)));

            if (settings.UsePersistentStorage)
            {
                container.RegisterType<IDeskStorage, LiteDbDeskStorage>(
                    new ContainerControlledLifetimeManager(),
                    new InjectionFactory(c => new LiteDbDeskStorage($"Filename={liteDbFilePath}")));
            }
            else
            {
                container.RegisterType<IDeskStorage, MemoryDeskStorage>(new ContainerControlledLifetimeManager());
            }

            return container;
        }
    }
}