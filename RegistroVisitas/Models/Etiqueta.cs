namespace RegistroVisitas.Models
{
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Etiqueta
    {
        [Key]
        public int id_etiqueta { get; set; }
        [Required]

        public int id_visita { get; set; }
        public int id_visitante { get; set; }
        public int id_empleado { get; set; }
        public int id_compañia { get; set; }
    }
}
