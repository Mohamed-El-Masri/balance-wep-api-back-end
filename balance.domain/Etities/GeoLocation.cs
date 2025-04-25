using System.ComponentModel.DataAnnotations.Schema;
using balance.domain.Common;

namespace balance.domain
{
    public class GeoLocation : BaseEntity
    {
        // Id is inherited from BaseEntity
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
     
        [ForeignKey("Property")]
        public int PropertyId { get; set; } 
        public Property Property { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
