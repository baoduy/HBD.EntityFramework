using HBD.EntityFramework.DbContexts.DbEntities;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFramework.Sample.DbEntities
{
    public class EmailAddessDb : DbEntity
    {
        public int PersonId { get; set; }

        [Required]
        [MaxLength(254)]
        public string Name { get; set; }

        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string Email { get; set; }

        public virtual PersonDb Person { get; set; }
    }
}