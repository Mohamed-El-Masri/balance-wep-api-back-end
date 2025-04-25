using System.ComponentModel.DataAnnotations.Schema;
using balance.domain.Common;

namespace balance.domain
{
    public class Location : BaseEntity
    {
        // Id is inherited from BaseEntity
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
