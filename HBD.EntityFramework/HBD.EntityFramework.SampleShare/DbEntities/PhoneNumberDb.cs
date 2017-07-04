using HBD.EntityFramework.DbContexts.DbEntities;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFramework.Sample.DbEntities
{
    public class PhoneNumberDb : DbEntity
    {
        public int PersonId { get; set; }

        [Required]
        [MaxLength(254)]
        public string Name { get; set; }

        [Required]
        [MaxLength(254)]
        public string PhoneNo { get; set; }

        public virtual PersonDb Person { get; set; }
    }
}