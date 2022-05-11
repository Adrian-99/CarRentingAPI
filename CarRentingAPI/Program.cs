using CarRentingAPI.Data;
using CarRentingAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CarRepository>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<RentalRepository>();

builder.Services.AddDbContext<CarRentingContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"))
    );

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
