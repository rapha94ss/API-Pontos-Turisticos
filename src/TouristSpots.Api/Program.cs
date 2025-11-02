using Microsoft.EntityFrameworkCore;
using TouristSpots.Application.Services;
using TouristSpots.Domain.Repositories;
using TouristSpots.Infrastructure.Data;
using TouristSpots.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Connection string via appsettings.json
var cs = builder.Configuration.GetConnectionString("DefaultConnection");

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(cs));

// DI de Repositório e Service
builder.Services.AddScoped<ITouristSpotRepository, TouristSpotRepository>();
builder.Services.AddScoped<ITouristSpotService, TouristSpotService>();

// --- Configurar CORS para desenvolvimento ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// --------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar CORS antes de mapear os controllers
app.UseCors("DevCors");

app.MapControllers();

app.Run();