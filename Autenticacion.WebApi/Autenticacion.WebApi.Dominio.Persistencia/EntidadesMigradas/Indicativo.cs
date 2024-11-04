using System;
using System.Collections.Generic;

namespace Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;

public partial class Indicativo
{
    public long IdIndicativo { get; set; }

    public string? Codigo { get; set; }

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
