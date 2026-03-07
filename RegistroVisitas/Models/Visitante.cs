namespace RegistroVisitas.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Visitante")]
    public class Visitante
    {
        [Key]
        public int id_visitante { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; } = string.Empty;

        [Display(Name = "Apellido paterno")]
        [Required]
        public string apellido_p { get; set; } = string.Empty;

        [Display(Name = "Apellido materno")]
        [Required]
        public string apellido_m { get; set; } = string.Empty;

        [Display(Name = "Correo")]
        [Required]
        public string correo { get; set; } = string.Empty;

        [Display(Name = "Foto")]
        [Required]
        public string foto { get; set; } = string.Empty;

        [Display(Name = "Asunto")]
        [Required]
        public string asunto { get; set; } = string.Empty;

        [Display(Name = "Compañía")]
        [Required]
        public int? id_compañia { get; set; }

        [Display(Name = "Empleado")]
        public int? id_empleado { get; set; }

        public ICollection<Visita> Visitas { get; set; } = new List<Visita>();

        [ForeignKey("id_compañia")]
        public Compañia? Compañia { get; set; }

        [ForeignKey("id_empleado")]
        public Empleado? Empleado { get; set; }
    }
}
