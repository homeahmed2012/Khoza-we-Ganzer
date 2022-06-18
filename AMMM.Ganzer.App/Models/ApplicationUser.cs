using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AMMM.Ganzer.App.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        
        [Required]
        public DateOnly BirthDate { get; set; }
        
        [Required]
        public int Governorate { get; set; }
        
        [Required]
        public int District { get; set; }
        
        [Required]
        public int FitnessLevel { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string? EmergencyPhone { get; set; }
        
        public DateOnly DateOfRidingBike { get; set; }
        
        public DateOnly DateOfJoiningTheGroup { get; set; }

        [Required]
        public int Points { get; set; }

        public string? ProfilePicture { get; set; }


        public virtual ICollection<Ride> Rides { get; set; }
    }
}
