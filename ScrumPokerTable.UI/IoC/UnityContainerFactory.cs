using Microsoft.Practices.Unity;
using ScrumPokerTable.UI.Providers;
using ScrumPokerTable.UI.Providers.Naming;
using ScrumPokerTable.UI.Providers.Storage;

namespace ScrumPokerTable.UI.IoC
{
    public static class UnityContainerFactory
    {
        public static IUnityContainer Create()
        {
            return new UnityContainer()
                .RegisterType<IDeskStorage, MemoryDeskStorage>(new ContainerControlledLifetimeManager())
                .RegisterType<IDeskNameProvider, DeskNameProvider>()
                .RegisterType<IDeskProvider, DeskProvider>();
        }
    }
}