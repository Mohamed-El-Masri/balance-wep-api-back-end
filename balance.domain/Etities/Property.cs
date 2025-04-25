using System.ComponentModel.DataAnnotations.Schema;
using balance.domain.Common;
using balance.domain.Enums;

namespace balance.domain
{
    public class Property : BaseEntity
    {
        // Id is inherited from BaseEntity
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Address { get; set; }

        public GeoLocation GeoLocation { get; set; }
        public Status Status { get; set; } // متاح - تم البيع - تم الحجز
        public offer_type offer_type { get; set; } //new
        public DateTime PostedDate { get; set; }

        [ForeignKey("PropertyType")]
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }

        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
        
        [ForeignKey("Agent")]
        public string AgentId { get; set; }
        public Applicationuser Agent { get; set; }
        
        // IsActive is inherited from BaseEntity
        public int? FloorNumber { get; set; }
        public int? TotalFloors { get; set; }
        public bool? HasParking { get; set; }
        public ICollection<PropertyImage> Images { get; set; }
        public ICollection<PropertyFeature> Features { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
