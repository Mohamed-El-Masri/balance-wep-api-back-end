using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace balance.domain
{
    public class Applicationuser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public ICollection<Favorite> Favorites { get; set; }
        [InverseProperty("Agent")]
        public ICollection<Property> Properties { get; set; }
        [InverseProperty("Agent")]
        public ICollection<Project> Projects { get; set; }
        
        // Full name property
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
