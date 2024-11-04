using System;
using System.Collections.Generic;

namespace Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;

public partial class Persona
{
    public long IdPersona { get; set; }

    public long IdIndicativo { get; set; }

    public long IdCiudad { get; set; }

    public string PrimerNombre { get; set; } = null!;

    public string? SegundoNombre { get; set; }

    public string PrimerApellido { get; set; } = null!;

    public string? SegundoApellido { get; set; }

    public string Telefono { get; set; } = null!;

    public string? NombreFoto { get; set; }

    public bool? EstadoEliminado { get; set; }

    public string UsuarioQueRegistra { get; set; } = null!;

    public string? UsuarioQueActualiza { get; set; }

    public DateOnly FechaDeRegistro { get; set; }

    public TimeOnly HoraDeRegistro { get; set; }

    public string IpDeRegistro { get; set; } = null!;

    public DateOnly? FechaDeActualizado { get; set; }

    public TimeOnly? HoraDeActualizado { get; set; }

    public string? IpDeActualizado { get; set; }

    public byte[]? Foto { get; set; }

    public virtual Indicativo IdIndicativoNavigation { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
