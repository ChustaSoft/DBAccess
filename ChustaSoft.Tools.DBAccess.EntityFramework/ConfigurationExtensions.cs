namespace ChustaSoft.Tools.DBAccess
{

#if NETCORE

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigurationExtensions
    {

        public static IServiceCollection RegisterDbAccess<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            
            return services;
        }

    }

#endif


#if NETFRAMEWORK

    using Unity;
    using System.Data.Entity;

    public static class ConfigurationExtensions
    {

        public static UnityContainer RegisterDbAccess<TContext>(this UnityContainer container)
            where TContext : DbContext
        {
            container.RegisterType<IUnitOfWork, UnitOfWork<TContext>>();

            return container;
        }

    }

#endif
}
