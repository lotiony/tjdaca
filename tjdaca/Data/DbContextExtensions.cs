using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace tjdaca.Data
{
    public static class DbContextExtensions
    {
        public static IConfigurationRoot? Configuration { get; set; }

        public static IServiceCollection AddDatabaseContext(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            services.AddDbContext<tjdacaContext>(options => options.UseMySql(Configuration.GetConnectionString("MysqlDbConnectionString"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.38-mysql")));

            return services;
        }
    }
}
