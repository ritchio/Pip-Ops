namespace HelpDesk.Constants
{
    public class AccionLogTicket
    {
        public const string Crear = "CREÓ TICKET";
        public const string Editar = "EDITÓ TICKET";
        public const string Asignar = "ASIGNÓ TICKET";
        public const string Tomo = "TOMÓ TICKET";
        public const string Cerrar = "CERRÓ TICKET";
        public const string Validar = "VALIDÓ TICKET";
    
        public static string AccionEstado(int estadoTicket)
        {
            return estadoTicket switch
            {
                EstadoTicket.PorAutorizar => "POR AUTORIZAR",
                EstadoTicket.Nuevo => "NUEVO",
                EstadoTicket.Abierto => "ABIERTO",
                EstadoTicket.Pendiente => "PENDIENTE",
                EstadoTicket.Cerrado => AccionLogTicket.Cerrar,
                EstadoTicket.Validado => AccionLogTicket.Validar,
                EstadoTicket.Rechazado => "RECHAZADO",
                _ => "ESTADO DESCONOCIDO"
            };
        }
    }
    

}
