using System;
using System.Collections.Generic;

namespace Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;

public partial class Role
{
    public long IdRol { get; set; }

    public string? Nombre { get; set; }

    public bool? EsSuperUsuario { get; set; }

    public bool? EstadoEliminado { get; set; }

    public string UsuarioQueRegistra { get; set; } = null!;

    public string? UsuarioQueActualiza { get; set; }

    public DateOnly FechaDeRegistro { get; set; }

    public TimeOnly? HoraDeRegistro { get; set; }

    public string IpDeRegistro { get; set; } = null!;

    public DateOnly? FechaDeActualizado { get; set; }

    public TimeOnly? HoraDeActualizado { get; set; }

    public string? IpDeActualizado { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
