using DateOnlyTimeOnly.AspNet.Converters;
using VacationModule.Core.ServiceContracts;
using VacationModule.Core.Services;
using Microsoft.EntityFrameworkCore;
using VacationModule.Infrastructure.Context;
using VacationModule.Core.Domain.RepositoryContracts;
using VacationModule.Infrastructure.Repositories;
using VacationModule.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// repositories
builder.Services.AddScoped<INationalHolidayRepository, NationalHolidayRepository>();

// add new functionality respecting open-closed principle (don't modify an existing class, unless fixing a bug)
builder.Services.AddScoped<NationalHolidayRepository, NationalHolidayRepository>();
builder.Services.AddScoped<INationalHolidayUpdateRepository, NationalHolidayUpdateRepository>();
builder.Services.AddScoped<IVacationRepository, VacationRepository>();
// services
builder.Services.AddScoped<INationalHolidaysService, NationalHolidaysService>();
builder.Services.AddScoped<IVacationsService, VacationsService>();
// dbContext
builder.Services.AddDbContext<ApplicationDbContext>
    (options =>
    {
        options.UseSqlServer(builder.Configuration
            .GetConnectionString("DefaultConnection")
            ,x => x.UseDateOnlyTimeOnly());
    });

// Enable Identiy with ApplicationUser for storing user details and ApplicationRole for storing role details
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        // Reduce password complexity
        options.Password.RequiredLength = 3; 
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddDefaultTokenProviders()
    // use Entity Framework to store the data in ApplicationDbContext
    .AddEntityFrameworkStores<ApplicationDbContext>()
    // configure the creation of the Repository
    // for users
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    // for roles
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Reading Identity cookie

app.UseAuthorization();

//app.UseRouting();

app.MapControllers();

app.Run();

public partial class Program { } // makes the auto-generated Program accesible to the developer
