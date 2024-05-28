using BankApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BankApplication.Controllers;

var builder = WebApplication.CreateBuilder(args);

//Register db context
builder.Services.AddDbContext<BankDbContext>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("BankDatabase"));   
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();

app.MapControllers();

app.Run();
