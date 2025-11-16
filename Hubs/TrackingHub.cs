using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BackendHorus.Hubs
{
    /// <summary>
    /// Hub de SignalR para enviar actualizaciones en tiempo real
    /// sobre posiciones, incidentes y cambios de estado de los recorridos.
    /// </summary>
    public class TrackingHub : Hub
    {
        // Prefijo común para los grupos por microruta
        public const string MicrorutaGroupPrefix = "microruta-";

        /// <summary>
        /// El supervisor se une al grupo de una microruta específica
        /// para recibir solo los eventos de esa microruta.
        /// </summary>
        public Task JoinMicrorutaGroup(int microrutaId)
        {
            var groupName = MicrorutaGroupPrefix + microrutaId;
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// El supervisor deja de escuchar una microruta.
        /// </summary>
        public Task LeaveMicrorutaGroup(int microrutaId)
        {
            var groupName = MicrorutaGroupPrefix + microrutaId;
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// Método opcional por si quieren que el front confirme que está conectado.
        /// No hace nada, solo sirve de ping.
        /// </summary>
        public Task Ping()
        {
            return Clients.Caller.SendAsync("Pong");
        }
    }
}