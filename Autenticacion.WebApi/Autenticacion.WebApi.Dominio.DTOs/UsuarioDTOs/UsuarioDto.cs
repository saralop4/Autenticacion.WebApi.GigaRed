
namespace Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;

    public class UsuarioDto
    {
        public long IdRol { get; set; } = 1;
        public long IdIndicativo { get; set; }
        public long IdCiudad { get; set; }
        public string? PrimerNombre { get; set; } 
        public string? SegundoNombre { get; set; } 
        public string? PrimerApellido { get; set; } 
        public string? SegundoApellido { get; set; }
        public string? Telefono { get; set; }
        public byte[]? Foto { get; set; }
        public string? NombreFoto { get; set; }
        public string? Correo { get; set; }
        public string? Contraseña { get; set; }
        public string? UsuarioQueRegistra { get; set; }
        public string? IpDeRegistro { get; set; } 
}
