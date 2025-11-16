using BackendHorus.Data;
using BackendHorus.Hubs;
using BackendHorus.Services.Implementations;
using BackendHorus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --------------------------
// 1. DbContext (SQL Server)
// --------------------------
builder.Services.AddDbContext<MicrorutasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --------------------------
// 2. Controllers + SignalR
// --------------------------
builder.Services.AddControllers();
builder.Services.AddSignalR();

// --------------------------
// 3. Servicios de la app
// --------------------------
builder.Services.AddScoped<IRutasService, RutasService>();
builder.Services.AddScoped<ITrackingService, TrackingService>();
builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddScoped<IRecolectoresService, RecolectoresService>();

// --------------------------
// 4. Swagger / OpenAPI
// --------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Backend Horus API",
        Version = "v1",
        Description = "API para monitoreo de microrrutas y recolección"
    });
});

// --------------------------
// 5. CORS (abierto para front)
// --------------------------
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

// ----------------------------------------
// 6. Migraciones + Seed de datos en inicio
// ----------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var db = services.GetRequiredService<MicrorutasDbContext>();

        // Aplica migraciones pendientes
        db.Database.Migrate();

        // Seed de datos iniciales
        await SeedData.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        // Loguea el error si quieres (en consola por ahora)
        Console.WriteLine($"Error al inicializar la BD: {ex.Message}");
        throw;
    }
}

// --------------------------
// 7. Middleware pipeline
// --------------------------

// ⚠️ Habilitamos Swagger SIEMPRE (también en Azure)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Horus API v1");
    // Deja el prefix por defecto "swagger":
    // la URL será https://tu-app.azurewebsites.net/swagger
});

// CORS
app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthorization();

// Rutas API + SignalR
app.MapControllers();
app.MapHub<TrackingHub>("/hubs/tracking");

app.Run();
