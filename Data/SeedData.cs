using System.Text.Json;
using BackendHorus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackendHorus.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MicrorutasDbContext>();

            // Aplica migraciones pendientes
            await context.Database.MigrateAsync();

            if (context.Macrorutas.Any())
            {
                // Ya hay datos, no hacemos nada
                return;
            }

            // 1) Macroruta
            var macroVerde = new Macroruta
            {
                Nombre = "Macroruta Verde",
                ColorHex = "#22c55e"
            };

            context.Macrorutas.Add(macroVerde);
            await context.SaveChangesAsync();

            // 2) Microruta demo (coordenadas inventadas cerca de Santa Cruz)
            var points = new[]
            {
                new { lat = -17.7830, lng = -63.1820 },
                new { lat = -17.7850, lng = -63.1800 },
                new { lat = -17.7870, lng = -63.1830 },
                new { lat = -17.7890, lng = -63.1810 }
            };
            var polylineJson = JsonSerializer.Serialize(points);

            var microAzul = new Microruta
            {
                Nombre = "Microruta Azul 1",
                MacrorutaId = macroVerde.Id,
                PolylineJson = polylineJson
            };

            context.Microrutas.Add(microAzul);

            // 3) Recolector demo
            var recolector = new Recolector
            {
                Nombre = "Ruddy Demo",
                Activo = true
            };

            context.Recolectores.Add(recolector);

            await context.SaveChangesAsync();
        }
    }
}