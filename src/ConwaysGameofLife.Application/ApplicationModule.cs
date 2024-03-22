using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using ConwaysGameofLife.Application.Services;

namespace ConwaysGameofLife.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModules(this IServiceCollection services)
        {
            return services
                .AddTransient<IRulesService, RulesService>()
                .AddTransient<IGameOfLifeService, GameOfLifeService>()                
                .AddAWSService<IAmazonDynamoDB>()
                .AddTransient<IDynamoDBContext, DynamoDBContext>();
        }
    }
}
