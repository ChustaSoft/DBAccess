#if NETCORE
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChustaSoft.Tools.DBAccess
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection RegisterDbAccess<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.AddScoped<IAsyncUnitOfWork, AsyncUnitOfWork<TContext>>();

            return services;
        }

    }
}
#endif