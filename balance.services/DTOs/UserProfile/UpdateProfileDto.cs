namespace balance.services.DTOs.UserProfile
{
    public class UpdateProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Bio { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
