namespace RegistroVisitas.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Compañia")]
    public class Compañia
    {
        [Key]
        public int id_compañia { get; set; }

        [Required]
        public string nombre { get; set; } = string.Empty;
    }
}
