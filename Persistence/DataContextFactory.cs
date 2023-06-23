using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Repositories
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

             IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../API"))
                 .AddJsonFile("appsettings.json")
                 .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                 .Build();

            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);

            return new DataContext(builder.Options);
        }
    }
}
