using ConwaysGameofLife.API.Filters;
using ConwaysGameofLife.Application;
using ConwaysGameofLife.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace ConwaysGameofLife.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            var awsOptions = builder.Configuration.GetAWSOptions();            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);                
            });
            builder.Services.AddApplicationModules();
            builder.Services.AddInfrastructureModule();
            builder.Services.AddDefaultAWSOptions(awsOptions);
            builder.Services.AddMediatR(config =>
                                        config.RegisterServicesFromAssembly(typeof(ApplicationModule).Assembly));
            builder.Services.AddMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}