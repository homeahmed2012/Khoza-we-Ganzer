using System.ComponentModel.DataAnnotations;

namespace AMMM.Ganzer.App.Models
{
    public class Ride
    {
        [Required]
        public int RideID { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [MaxLength(150)]
        public string? Name { get; set; }

        [Required]
        public int RideType { get; set; }

        [Required]
        public DateOnly RideDate { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public TimeOnly RideTime { get; set; }

        [Required]
        public int Distance { get; set; }

        [Required]
        public string? GatheringPoint { get; set; }

        [Required]
        public string? Distenation { get; set; }

        public int Temperature { get; set; }

        public string? WindDirection { get; set; }

        public int? WindIntensity { get; set; }

        public int? Rain { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
