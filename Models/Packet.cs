using System.ComponentModel.DataAnnotations;

namespace tz_tubes.Models
{
    public class Packet
    {
        public int PacketId { get; set; }
        public int PacketNumber { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
