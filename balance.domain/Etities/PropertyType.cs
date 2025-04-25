using Microsoft.EntityFrameworkCore.Metadata.Internal;
using balance.domain.Common;

namespace balance.domain
{
    public class PropertyType : BaseEntity
    {
        // Id is inherited from BaseEntity
        public string Name { get; set; } // سكني - تجاري - إداري - فيلا - شاليه ...
        public string Description { get; set; }
        public ICollection<Property> Properties { get; set; }
    }
}
