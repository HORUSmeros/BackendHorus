using BackendHorus.Data;
using BackendHorus.Hubs;
using BackendHorus.Services.Implementations;
using BackendHorus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔌 DbContext
builder.Services.AddDbContext<MicrorutasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Si quieren SQLite, sería algo como:
// options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllers();
builder.Services.AddSignalR();

// 🔧 Services
builder.Services.AddScoped<IRutasService, RutasService>();
builder.Services.AddScoped<ITrackingService, TrackingService>();
builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddScoped<IRecolectoresService, RecolectoresService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS abierto para el front del hackathon
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Seed + migraciones
await SeedData.InitializeAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapHub<TrackingHub>("/hubs/tracking");

app.Run();