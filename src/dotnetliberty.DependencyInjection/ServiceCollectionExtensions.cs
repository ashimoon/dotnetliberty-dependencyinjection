using System;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetliberty.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonFactory<T, TFactory>(this IServiceCollection collection)
            where T : class where TFactory : class, IServiceFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddSingletonFactory<T, TFactory>(this IServiceCollection collection, TFactory factory)
            where T : class where TFactory : class, IServiceFactory<T>
        {
            return AddInternal<T, TFactory>(collection, p => factory, ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddTransientFactory<T, TFactory>(this IServiceCollection collection)
            where T : class where TFactory : class, IServiceFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Transient);
        }

        public static IServiceCollection AddTransientFactory<T, TFactory>(this IServiceCollection collection, TFactory factory)
            where T : class where TFactory : class, IServiceFactory<T>
        {
            return AddInternal<T, TFactory>(collection, p => factory, ServiceLifetime.Transient);
        }

        public static IServiceCollection AddScopedFactory<T, TFactory>(this IServiceCollection collection)
            where T : class where TFactory : class, IServiceFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddScopedFactory<T, TFactory>(this IServiceCollection collection, TFactory factory)
            where T : class where TFactory : class, IServiceFactory<T>
        {
            return AddInternal<T, TFactory>(collection, p => factory, ServiceLifetime.Scoped);
        }

        private static IServiceCollection AddInternal<T, TFactory>(
            this IServiceCollection collection,
            Func<IServiceProvider, TFactory> factoryProvider,
            ServiceLifetime lifetime) where T : class where TFactory : class, IServiceFactory<T>
        {
            Func<IServiceProvider, object> factoryFunc = provider =>
            {
                var factory = factoryProvider(provider);
                return factory.Build();
            };
            var descriptor = new ServiceDescriptor(
                typeof(T), factoryFunc, lifetime);
            collection.Add(descriptor);
            return collection;
        }
    }
}