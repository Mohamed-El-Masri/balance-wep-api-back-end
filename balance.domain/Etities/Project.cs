using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using balance.domain.Common;

namespace balance.domain
{
    public class Project : BaseEntity
    {
        // Id is inherited from BaseEntity
        public string Name { get; set; }
        
        [ForeignKey("Agent")] 
        public string AgentId { get; set; }
        public Applicationuser Agent { get; set; }
    
        // CreatedAt is inherited from BaseEntity
        // IsActive is inherited from BaseEntity
        
        public Location Location { get; set; }
        public string Description { get; set; }
        public DateTime DeliveryDate { get; set; }
        public ICollection<Property> Properties { get; set; }
    }
}
