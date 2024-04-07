using System.ComponentModel.DataAnnotations;
namespace tz_tubes.Models
{
    public class Pipe
    {
        public int PipeId { get; set; }

        [Required]
        public bool Quality { get; set; }

        public int SteelGrade { get; set; }

        public double Size { get; set; }

        public double Weight { get; set; }

        public int? PacketId { get; set; }

    }
}
