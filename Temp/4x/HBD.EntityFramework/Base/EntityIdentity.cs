using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EntityFramework.Base
{
    public class EntityIdentity : Entity<int>
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }
    }
}