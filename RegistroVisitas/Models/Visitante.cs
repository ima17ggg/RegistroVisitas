namespace RegistroVisitas.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Visitante")]
    public class Visitante
    {
        [Key]
        public int id_visitante { get; set; }

        [Required]
        public string nombre { get; set; } = string.Empty;

        public string apellido_p { get; set; } = string.Empty;
        public string? apellido_m { get; set; }
        public string? correo { get; set; }
        public string? foto { get; set; }
        public string? asunto { get; set; }

        [Required(ErrorMessage = "Seleccione una compañía válida o ingrese una nueva.")]
        public int? id_compañia { get; set; }

        public int? id_empleado { get; set; }

        public ICollection<Visita> Visitas { get; set; } = new List<Visita>();

        [ForeignKey("id_compañia")]
        public Compañia? Compañia { get; set; }

        [ForeignKey("id_empleado")]
        public Empleado? Empleado { get; set; }
    }
}
