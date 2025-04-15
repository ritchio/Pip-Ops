namespace HelpDesk.Models.Utils
{
    /// <summary>
    /// Se encarga de estandarizar la respuesta en casos necesarios para su procesamiento en el frontend
    /// </summary>
    public class RespuestaGenerica
    {
        public bool Exito { get; set; }
        public string? Mensaje { get; set; }
        public string? Datos { get; set; }
        public string? Redireccion { get; set; }
    }
}
