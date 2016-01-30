using Microsoft.Practices.Unity;
using ScrumPokerTable.UI.DataAccess.Providers;

namespace ScrumPokerTable.UI.IoC
{
    public static class UnityContainerFactory
    {
        public static IUnityContainer Create()
        {
            return new UnityContainer()
                .RegisterType<IDeskNameProvider, DeskNameProvider>()
                .RegisterType<IDeskProvider, InMemoryDeskProvider>(new ContainerControlledLifetimeManager());
        }
    }
}