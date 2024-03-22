using ConwaysGameofLife.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ConwaysGameofLife.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IRepository<>), typeof(Repository<>)); ;
        }
    }
}
