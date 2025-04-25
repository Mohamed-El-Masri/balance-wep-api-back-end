using System.ComponentModel.DataAnnotations.Schema;
using balance.domain.Common;

namespace balance.domain
{
    public class PropertyFeature : BaseEntity
    {
        // Id is inherited from BaseEntity
        public string Name { get; set; }
        public string Description { get; set; }
        // IsAvailable is replaced by IsActive from BaseEntity
        
        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}