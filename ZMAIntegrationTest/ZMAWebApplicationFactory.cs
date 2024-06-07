using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZMA.Data;

namespace ZMAIntegrationTest;

public class ZMAWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ZMAContext>));

            services.Remove(dbContextDescriptor);
            
            var dbContextServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ZMAContext));
            
            services.Remove(dbContextServiceDescriptor);
            services.RemoveAll(typeof(DbContextOptions<ZMAContext>));
            
            var config = new ConfigurationBuilder()
                .AddUserSecrets<ZMAWebApplicationFactory>()
                .Build();
            
            services.AddDbContext<ZMAContext>((container, options) =>
            {
                options.UseSqlServer(config["TestConnectionString"] != null
                    ? config["TestConnectionString"]:Environment.GetEnvironmentVariable("TESTCONNECTIONSTRING"));
            });

            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ZMAContext>();
            var authSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            authSeeder.AddRole();
            authSeeder.AddHost();
        });
    }
}