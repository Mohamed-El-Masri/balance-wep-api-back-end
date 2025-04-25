using System.ComponentModel.DataAnnotations;

namespace balance.services.DTOs.Authentication
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string? Address { get; set; }
        
        // Default role is Customer, can be overridden during admin registration
        public string Role { get; set; } = "Customer";
    }
}
