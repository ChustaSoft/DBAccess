﻿namespace ChustaSoft.Tools.DBAccess
{

#if (NETCORE || NET5_0 || NET6_0)

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigurationExtensions
    {

        public static IServiceCollection RegisterDbAccess<TContext, TKey>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork<TKey>, UnitOfWork<TContext, TKey>>();
            
            return services;
        }

        public static IServiceCollection RegisterDbAccess<TContext>(this IServiceCollection services)
           where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();

            return services;
        }

    }

#endif


#if NETFRAMEWORK

    using System.Data.Entity;
    using Unity;

    public static class ConfigurationExtensions
    {

        public static UnityContainer RegisterDbAccess<TContext, TKey>(this UnityContainer container)
            where TContext : DbContext
        {
            container.RegisterType<IUnitOfWork<TKey>, UnitOfWork<TContext, TKey>>();

            return container;
        }

        public static UnityContainer RegisterDbAccess<TContext>(this UnityContainer container)
           where TContext : DbContext
        {
            container.RegisterType<IUnitOfWork, UnitOfWork<TContext>>();

            return container;
        }

    }

#endif
}
