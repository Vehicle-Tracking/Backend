using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Avt.Web.Backend
{
    public static class ServiceLocator
    {
        private static IServiceCollection _services;
        private static Lazy<IServiceProvider> _serviceProvider;

        public static void Init(IServiceCollection serviceCollection) 
        {
           Init(serviceCollection, serviceCollection.BuildServiceProvider());
        }

        public static void Init(IServiceCollection serviceCollection, IServiceProvider serviceProvider)
        {
            if (serviceCollection != null)
            {
                _services = serviceCollection;
                _serviceProvider = new Lazy<IServiceProvider>(() => serviceProvider, LazyThreadSafetyMode.ExecutionAndPublication);
            }
        }

        public static IServiceCollection GetServiceCollection()
        {
            return _services;
        }

        public static IServiceProvider GetServiceProvider()
        {
            return _serviceProvider?.Value;
        }

        public static T Resolve<T>()
        {
            return _serviceProvider.Value.GetService<T>();
        }
    }
}
