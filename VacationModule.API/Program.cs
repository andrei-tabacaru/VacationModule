using DateOnlyTimeOnly.AspNet.Converters;
using VacationModule.Core.ServiceContracts;
using VacationModule.Core.Services;
using Microsoft.EntityFrameworkCore;
using VacationModule.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddSingleton<INationalHolidaysService, NationalHolidaysService>();
/*builder.Services.AddDbContext<ApplicationDbContext>
    (options => {
        options.UseSqlServer(builder.Configuration
            .GetConnectionString("DefaultConnection"));
    });*/
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

app.UseAuthorization();

app.MapControllers();

app.Run();
