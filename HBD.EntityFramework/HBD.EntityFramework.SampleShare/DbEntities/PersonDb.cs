using HBD.EntityFramework.DbContexts.DbEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFramework.Sample.DbEntities
{
    public class PersonDb : DbEntity
    {
        public PersonDb()
        {
            Addresses = new HashSet<AddressDb>();
            PhoneNumbers = new HashSet<PhoneNumberDb>();
            EmailAddresses = new HashSet<EmailAddessDb>();
        }

        [Required]
        [MaxLength(254)]
        public string FirstName { get; set; }

        [MaxLength(254)]
        public string LastName { get; set; }

        public virtual HashSet<AddressDb> Addresses { get; }
        public virtual HashSet<PhoneNumberDb> PhoneNumbers { get; }
        public virtual HashSet<EmailAddessDb> EmailAddresses { get; }
    }
}