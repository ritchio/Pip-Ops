using HelpDesk.Data;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Ticket.Services
{
    public class TicketAutoValidarService(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly TimeSpan _intervalo = TimeSpan.FromHours(1); // cada dos hora

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        var fechaLimite = DateTime.Now.AddHours(-48);// 48 horas atrás

                        var tickets = await dbContext.Doc_Ticket
                            .Where(t => t.IdEstadoTicket == 5 && t.FechaCerrado <= fechaLimite)
                            .ToListAsync();

                        if (tickets.Count != 0)
                        {
                            foreach (var ticket in tickets)
                            {
                                //Console.WriteLine($"Ticket {ticket.IdTicket} auto-validado por inactividad de 48 horas.");
                                ticket.IdEstadoTicket = 6;
                            }

                            await dbContext.SaveChangesAsync();
                            //Console.WriteLine("Tickets auto-validados correctamente.");
                        }
                        else { 
                            //Console.WriteLine("No hay tickets para auto-validar en este momento."+fechaLimite);
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Error en TicketAutoValidarService: {ex.Message}");
                }

                await Task.Delay(_intervalo, stoppingToken);
            }
        }
    }
}
