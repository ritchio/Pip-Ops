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

            [PermisoDescripcion("Editar usuario existente")]
            public const string Editar = "Usuario.Editar";

            [PermisoDescripcion("Eliminar usuario")]
            public const string Eliminar = "Usuario.Eliminar";
        }

        public static class Ticket
        {
            [PermisoDescripcion("Ver tickets")]
            public const string Ver = "Ticket.Ver";

            [PermisoDescripcion("Asignar tickets")]
            public const string Asignar = "Ticket.Asignar";

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
