using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;

namespace ScrumPokerTable.UI.IoC
{
    public class SignalrDependencyResolver : DefaultDependencyResolver
    {
        private readonly IUnityContainer _container;

        public SignalrDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch
            {
                return base.GetService(serviceType);
            }
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Concat(base.GetServices(serviceType));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}