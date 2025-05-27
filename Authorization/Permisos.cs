namespace HelpDesk.Authorization
{
    public static class Permisos
    {
        public static class Usuario
        {
            [PermisoDescripcion("Ver lista de usuarios")]
            public const string Ver = "Usuario.Ver";

            [PermisoDescripcion("Crear nuevo usuario")]
            public const string Crear = "Usuario.Crear";

            [PermisoDescripcion("Editar la información de usuario existente")]
            public const string Editar = "Usuario.Editar";

            [PermisoDescripcion("Activar o desactivar usuarios")]
            public const string Desactivar = "Usuario.Desactivar";

            [PermisoDescripcion("Agregar o quitar permisos de usuarios")]
            public const string Permisos = "Usuario.Permisos";
        }

        public static class Ticket
        {
            [PermisoDescripcion("Crear tickets")]
            public const string Crear = "Ticket.Crear";

            [PermisoDescripcion("Asignar tickets a agentes de soporte")]
            public const string Asignar = "Ticket.Asignar";

            [PermisoDescripcion("Auto-asignar tickets")]
            public const string AutoAsignar = "Ticket.AutoAsignar";

            [PermisoDescripcion("Quitar asignación de un ticket a un agente y asignarlo a otro")]
            public const string ReAsignar = "Ticket.ReAsignar";

            [PermisoDescripcion("Marcar tickets como resueltos")]
            public const string Resolver = "Ticket.Resolver";
        }

        public static class Sistema
        {
            [PermisoDescripcion("Acceso total al sistema")]
            public const string AccesoTotal = "Sistema.Administrar";
        }
    }

}
