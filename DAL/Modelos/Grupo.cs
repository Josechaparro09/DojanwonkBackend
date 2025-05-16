using System;
using System.Collections.Generic;

namespace DAL.Modelos;

public partial class Grupo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
}
