using System.ComponentModel.DataAnnotations;

namespace LogisticsAssistant.Models
{
    public class TruckViewModel
    {
        [Required]
        public string LicensePlate { get; set; }
        [Required]
        public int Vmax { get; set; }
        [Required]
        public int DriverBreak { get; set; } // in minutes
        public int UserId { get; set; }
    }
}
