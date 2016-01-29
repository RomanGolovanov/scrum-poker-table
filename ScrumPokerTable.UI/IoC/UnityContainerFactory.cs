﻿using Microsoft.Practices.Unity;
using ScrumPokerTable.UI.DataAccess;
using ScrumPokerTable.UI.DataAccess.Providers;

namespace ScrumPokerTable.UI.IoC
{
    public static class UnityContainerFactory
    {
        public static IUnityContainer Create()
        {
            return new UnityContainer()
                .RegisterType<IDeskRepository, InMemoryDeskRepository>(new ContainerControlledLifetimeManager());
        }
    }
}