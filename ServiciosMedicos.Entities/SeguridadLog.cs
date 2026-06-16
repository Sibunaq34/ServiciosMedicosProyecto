namespace Servicios_Medicos.Entities
{
    public class SeguridadLog
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }

        public string NombreCompleto { get; set; }
        public string Password { get; set; }
        public string PasswordCifrada { get; set; }
        public int IdRol { get; set; } 
        public string Estado { get; set; }
        public string? NombreRol { get; set; }

        public int intentos_fallidos { get; set; }
    }

}
