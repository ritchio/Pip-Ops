namespace HelpDesk.Constants
{
    public class EstadoTicket
    {
        public const int PorAutorizar = 1;
        public const int Nuevo = 2;
        public const int Abierto = 3;
        public const int Pendiente = 4;
        public const int Cerrado = 5;
        public const int Validado = 6;
        public const int Rechazado = 7;

    

        public static string Texto(int estadoTicket)
        {
            return estadoTicket switch
            {
                EstadoTicket.PorAutorizar => "Por Autorizar",
                EstadoTicket.Nuevo => "Nuevo",
                EstadoTicket.Abierto => "Abierto",
                EstadoTicket.Pendiente => "Pendiente",
                EstadoTicket.Cerrado => "Cerrado",
                EstadoTicket.Validado => "Validado",
                EstadoTicket.Rechazado => "Rechazado",
                _ => "ESTADO DESCONOCIDO"
            };
        }

        public static string MensajeCorreo(int estadoTicket)
        {
            return estadoTicket switch
            {
                EstadoTicket.Pendiente => "ha cambiado el estatus a pendiente, el agente está en espera de su respuesta.",
                EstadoTicket.Cerrado => "ha sido cerrado, favor de verificar o validar.",
                EstadoTicket.Abierto => "ha sido tomado por un agente, favor de verificar.",
                EstadoTicket.Validado => "ha sido validado por el usuario.",
                EstadoTicket.Rechazado => "ha sido rechazado, favor de verificar.",
                _ => "favor de verificar."
            };
        }

    }
}