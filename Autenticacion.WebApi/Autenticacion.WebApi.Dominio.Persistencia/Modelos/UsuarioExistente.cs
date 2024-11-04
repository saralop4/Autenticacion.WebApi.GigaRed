namespace Autenticacion.WebApi.Dominio.Persistencia.Modelos
{
    public class UsuarioExistente
    {
        public long IdUsuario { get; set; }
        public long IdRol { get; set; }
        public string? NombreRol { get; set; }
        public string? Correo { get; set; } 
        public string? Contraseña { get; set; } 

    }
}
