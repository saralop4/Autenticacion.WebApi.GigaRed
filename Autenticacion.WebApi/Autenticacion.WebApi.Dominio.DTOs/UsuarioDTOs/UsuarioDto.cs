namespace Autenticacion.WebApi.Dominio.DTOs.UsuarioDTOs;

    public class UsuarioDto
    {
        public long IdRol { get; set; }
        public long IdIndicativo { get; set; }
        public long IdCiudad { get; set; }
        public string PrimerNombre { get; set; } = null!;
        public string? SegundoNombre { get; set; } 
        public string PrimerApellido { get; set; } = null!;
        public string? SegundoApellido { get; set; }
        public string Telefono { get; set; } = null!;
        public byte[]? Foto { get; set; }
        public string? NombreFoto { get; set; }
        public string Correo { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public string UsuarioQueRegistra { get; set; } = null!;
        public string IpDeRegistro { get; set; } = null!;
}
