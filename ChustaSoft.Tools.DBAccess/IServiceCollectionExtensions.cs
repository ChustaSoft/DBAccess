using ChustaSoft.Tools.DBAccess.Abstractions;
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

            return services;
        }

    }
}
