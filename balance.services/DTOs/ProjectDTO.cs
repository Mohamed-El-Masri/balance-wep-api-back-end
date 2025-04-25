namespace balance.services.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public LocationDTO Location { get; set; }
        public string Description { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int PropertiesCount { get; set; }
        public GeoLocationDTO GeoLocation { get; set; }
    }
}
