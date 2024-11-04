using System;
using System.Collections.Generic;

namespace Autenticacion.WebApi.Dominio.Persistencia.EntidadesMigradas;

public partial class Ciudade
{
    public long IdCiudad { get; set; }

    public long IdPais { get; set; }

    public string? Nombre { get; set; }

    public virtual Paise IdPaisNavigation { get; set; } = null!;
}
