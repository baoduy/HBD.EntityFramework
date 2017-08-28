using HBD.EntityFramework.GlobalShare.Core;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFramework.Sample.ContactManagement
{
    public sealed class Address : Value<Address>
    {
        public Address(int id, string blockNo, string street, string city, string postalCode, string country)
            : this(blockNo, street, city, postalCode, country)
        {
            Id = id;
        }

        public Address(string blockNo, string street, string city, string postalCode, string country)
        {
            BlockNo = blockNo;
            City = city;
            Country = country;
            PostalCode = postalCode;
            Street = street;
        }

        public int Id { get; }

        [Required]
        [StringLength(254)]
        public string BlockNo { get; }

        [StringLength(254)]
        public string Street { get; }

        [StringLength(254)]
        public string City { get; }

        [StringLength(254)]
        public string PostalCode { get; }

        [StringLength(254)]
        public string Country { get; }

        public override bool Equals(Address other)
        {
            return BlockNo == other.BlockNo
                && City == other.City
                && Country == other.Country
                && PostalCode == other.PostalCode
                && Street == other.Street;
        }
    }
}