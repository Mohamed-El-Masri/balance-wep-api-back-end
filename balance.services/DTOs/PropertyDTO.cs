using balance.domain.Enums;

namespace balance.services.DTOs
{
    public class PropertyDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Address { get; set; }
        public Status Status { get; set; }
        public offer_type OfferType { get; set; }
        public DateTime PostedDate { get; set; }
        public int PropertyTypeId { get; set; }
        public string PropertyTypeName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public bool IsActive { get; set; }
        public int? FloorNumber { get; set; }
        public int? TotalFloors { get; set; }
        public bool? HasParking { get; set; }
        public GeoLocationDTO GeoLocation { get; set; }
        public List<PropertyImageDTO> Images { get; set; }
        public List<PropertyFeatureDTO> Features { get; set; }
        public bool IsFavorite { get; set; }
    }
}
