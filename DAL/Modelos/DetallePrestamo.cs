using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Modelos;

public partial class DetallePrestamo
{
    public int Id { get; set; }
    public int IdArticulo { get; set; }

    public int Cantidad { get; set; }

    public int IdPrestamo { get; set; }
    [JsonIgnore]
    public virtual Articulo? IdArticuloNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Prestamo? IdPrestamoNavigation { get; set; } = null!;
}
