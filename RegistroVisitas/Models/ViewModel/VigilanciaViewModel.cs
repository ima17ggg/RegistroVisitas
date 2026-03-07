namespace RegistroVisitas.Models.ViewModels
{
    public class VigilanciaViewModel
    {
        public int id_visita { get; set; }

        public string nombre { get; set; } = string.Empty;
        public string apellido_p { get; set; } = string.Empty;
        public string? apellido_m { get; set; }

        public string? asunto { get; set; }

        public string? compania { get; set; }
        public string? empleado { get; set; }

        public string? foto { get; set; }

        public DateTime fecha_entrada { get; set; }

        public DateTime? fecha_salida { get; set; }

        public byte[]? QRCodeData { get; set; }
        }
}

