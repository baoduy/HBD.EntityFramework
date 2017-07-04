using HBD.EntityFramework.DbContexts.DbEntities;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFramework.Sample.DbEntities
{
    public class AddressDb : DbEntity
    {
        public int PersonId { get; set; }

        [Required]
        [MaxLength(254)]
        public string BlockNo { get; set; }

        [MaxLength(254)]
        public string Street { get; set; }

        [MaxLength(254)]
        public string City { get; set; }

        [MaxLength(254)]
        public string PostalCode { get; set; }

        [MaxLength(254)]
        public string Country { get; set; }

        public virtual PersonDb Person { get; set; }
    }
}