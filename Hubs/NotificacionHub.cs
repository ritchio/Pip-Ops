using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HelpDesk.Hubs
{
    public class NotificacionHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var identityName = Context.User?.Identity?.Name ?? "Anónimo";
            var userIdentifier = Context.UserIdentifier ?? "Sin identificador";

            Console.WriteLine($"[HUB] Usuario conectado:");
            Console.WriteLine($"       Identity.Name     : {identityName}");
            Console.WriteLine($"       UserIdentifier    : {userIdentifier}");

            await base.OnConnectedAsync();
        }
       
    }
}

