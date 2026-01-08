namespace RegistroVisitas.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Empleado")]
    public class Empleado
    {
        [Key]
        public int id_empleado { get; set; }

        [Required]
        public string nombre { get; set; } = string.Empty;
        public string apellido_p { get; set; } = string.Empty;
        public string apellido_m { get; set; } = string.Empty;
        public string posicion { get; set; } = string.Empty;
    }
}
