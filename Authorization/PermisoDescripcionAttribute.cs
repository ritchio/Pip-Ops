namespace HelpDesk.Authorization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class PermisoDescripcionAttribute : Attribute
    {
        public string Descripcion { get; }

        public PermisoDescripcionAttribute(string descripcion)
        {
            Descripcion = descripcion;
        }
    }

}
