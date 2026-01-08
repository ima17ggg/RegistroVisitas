namespace RegistroVisitas.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Visita")]
    public class Visita
    {
        [Key]
        public int id_visita { get; set; }

        [Required]
        public int id_visitante { get; set; }

        public DateTime fecha_entrada { get; set; }
        public DateTime? fecha_salida { get; set; }
    }
}
