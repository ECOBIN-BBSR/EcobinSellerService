using EcobinSellerService.Application.Features;
using EcobinSellerService.Application.Interfaces;
using EcobinSellerService.Infrastructure.Persistance;
using EcobinSellerService.Infrastructure.Repositories;
using EcobinSellerService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RegisterSellerHandler>();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database")));

// Infrastructure services
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();

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
