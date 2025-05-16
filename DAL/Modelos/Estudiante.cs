using System;
using System.Collections.Generic;

namespace DAL.Modelos;

public partial class Estudiante
{
    public string Id { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Eps { get; set; } = null!;

    public int IdRango { get; set; }

    public int IdGrupo { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public virtual ICollection<Examen> Examenes { get; set; } = new List<Examen>();

    public virtual Grupo IdGrupoNavigation { get; set; } = null!;

    public virtual Rango IdRangoNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
