using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace VacationModule.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");

            // For testing we want to use EntityFrameworkCore.InMemory, not EntityFrameworkCore.SqlServer
            // Remove existing ApplicationDbContext service 
            builder.ConfigureServices(services => { 
                // Search and return DbContextOptions of ApplicationDbContext
                // From already existing available serives
                var descripter = services.SingleOrDefault(temp => 
                // get the service type DbContextOptions of the ApplicationDbContext
                temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if(descripter != null)
                {
                    services.Remove(descripter);
                }

                // Create an in memory database for tesing eviroment
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    // This will be recreated everytime we run the integration tests, so we will have an empty DB for testing
                    options.UseInMemoryDatabase("DatabaseForTesting");
                });
            });
        }
    }
}
