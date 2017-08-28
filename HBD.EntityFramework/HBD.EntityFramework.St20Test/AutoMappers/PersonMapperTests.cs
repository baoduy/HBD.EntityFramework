using AutoMapper;
using FluentAssertions;
using HBD.EntityFramework.Sample.ContactManagement;
using HBD.EntityFramework.Sample.DbEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HBD.EntityFramework.TestSample.AutoMappers
{
    [TestClass]
    public class PersonMapperTests
    {
        [TestInitialize]
        public void Setup()
        {
            var a = SampleBootStrapper.Default;
        }

        [TestMethod]
        public void Can_Convert_Person_To_Contact()
        {
            var p = new PersonDb
            {
                Id = 1,
                FirstName = "Duy",
                LastName = "Hoang",
            };

            p.Addresses.Add(new AddressDb { Id = 1, BlockNo = "123", City = "SG", Country = "SG", PostalCode = "123456", Street = "HBD" });
            p.PhoneNumbers.Add(new PhoneNumberDb { Id = 1, Name = "Home", PhoneNo = "99999999" });
            p.EmailAddresses.Add(new EmailAddessDb { Id = 1, Name = "Persiona;", Email = "abc@123.com" });

            var c = Mapper.Map<Contact>(p);

            c.FirstName.Should().Be(p.FirstName);
            c.LastName.Should().Be(p.LastName);

            c.Addresses.Count.Should().Be(p.Addresses.Count);
            c.Phones.Count.Should().Be(p.PhoneNumbers.Count);
            c.Emails.Count.Should().Be(p.EmailAddresses.Count);

            c.Addresses.First().Id.Should().Be(1);
            c.Phones.First().Id.Should().Be(1);
            c.Emails.First().Id.Should().Be(1);
        }

        [TestMethod]
        public void Can_Convert_Contact_To_Person()
        {
            var c = new ContactService(null).CreateNew("Duy", "Hoang")
                .WithAddress(new Address(1,"123", "222", "SG", "1111", "SG"))
                .WithEmail(new EmailAddress(1,"Personal", "abc@123.com"))
                .WithPhone(new PhoneNumber(1,"Mobile", "123456"));


            var p = Mapper.Map<PersonDb>(c);

            p.FirstName.Should().Be(c.FirstName);
            p.LastName.Should().Be(c.LastName);

            p.Addresses.Count.Should().Be(c.Addresses.Count);
            p.PhoneNumbers.Count.Should().Be(c.Phones.Count);
            p.EmailAddresses.Count.Should().Be(c.Emails.Count);

            p.Addresses.First().Id.Should().Be(1);
            p.EmailAddresses.First().Id.Should().Be(1);
            p.PhoneNumbers.First().Id.Should().Be(1);
        }
    }
}