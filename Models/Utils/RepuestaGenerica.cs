namespace HelpDesk.Models.Utils
{
    /// <summary>
    /// Se encarga de estandarizar la respuesta en casos necesarios para su procesamiento en el frontend
    /// </summary>
    public class RespuestaGenerica<T>
    {
        public bool Exito { get; set; } = false;
        public string? Mensaje { get; set; }
        public T? Datos { get; set; }
        public string? Redireccion { get; set; }
        public string? Llamada { get; set; }
    }
}
