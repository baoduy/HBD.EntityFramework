using HBD.EntityFramework.GlobalShare.Core;
using HBD.Framework;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFramework.Sample.ContactManagement
{
    public sealed class EmailAddress : Value<EmailAddress>
    {
        public EmailAddress(string name, string email)
            : this(0, name, email)
        {
        }

        public EmailAddress(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public int Id { get; }

        [Required]
        [StringLength(254)]
        public string Name { get; }

        [Required]
        [StringLength(254)]
        [EmailAddress]
        public string Email { get; }

        public override bool Equals(EmailAddress other)
            => Email.EqualsIgnoreCase(other.Email);
    }
}