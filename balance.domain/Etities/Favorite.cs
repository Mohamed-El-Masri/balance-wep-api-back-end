using System.ComponentModel.DataAnnotations.Schema;
using balance.domain.Common;

namespace balance.domain
{
    public class Favorite : BaseEntity
    {
        // Id is inherited from BaseEntity

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public Applicationuser User { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public Property? Property { get; set; }

        // AddedDate replaced by CreatedAt from BaseEntity
        public string? Notes { get; set; }
    }
}