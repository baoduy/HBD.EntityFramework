using FluentAssertions;
using HBD.EntityFramework.DbContexts.Interfaces;
using HBD.EntityFramework.Exceptions;
using HBD.EntityFramework.Sample.ContactManagement;
using HBD.EntityFramework.Sample.DbEntities;
using HBD.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.TestSample.ContactManagement
{
    [TestClass]
    public class ContactServiceTests
    {
        [TestMethod]
        public void Can_Get_IContactService_From_Mef()
        {
            var service = SampleBootStrapper.GetExport<IContactService>();
            service.Should().NotBeNull();
        }

        [TestMethod]
        public void Create_New_Contact_And_Save_ToDb()
        {
            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contact = service.CreateNew("Duy", "Hoang");
                contact = service.AddOrUpdate(contact);
                contact.Id.Should().BeGreaterOrEqualTo(1);
            }

            using (var dbcontext = SampleBootStrapper.GetExport<IDbRepositoryFactory>())
            {
                dbcontext.For<PersonDb>().AsQueryable().Any(p => p.FirstName == "Duy" && p.LastName == "Hoang")
                    .Should().BeTrue();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanNot_Save_TheSame_Contact_Twice()
        {
            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contact = service.CreateNew("Duy", "Hoang");
                service.AddOrUpdate(contact);
                service.AddOrUpdate(contact);
            }
        }

        [TestMethod]
        public void Create_New_Contact_With_Address_Phone_Email_And_Save_ToDb()
        {
            int id = 0;

            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contact = service.CreateNew("Duy", "Hoang")
                    .WithAddress(new Address("123", "111", string.Empty, string.Empty, string.Empty))
                    .WithEmail(new EmailAddress("Personal", "abc@123.com"))
                    .WithPhone(new PhoneNumber("Mobile", "123456"));

                var savedContact = service.AddOrUpdate(contact);
                id = savedContact.Id;

                savedContact.FirstName.Should().Be("Duy");
                savedContact.LastName.Should().Be("Hoang");
                savedContact.Addresses.First().BlockNo.Should().Be("123");
                savedContact.Addresses.First().Id.Should().BeGreaterOrEqualTo(1);
                savedContact.Emails.First().Email.Should().Be("abc@123.com");
                savedContact.Emails.First().Id.Should().BeGreaterOrEqualTo(1);
                savedContact.Phones.First().Name.Should().Be("Mobile");
                savedContact.Phones.First().Id.Should().BeGreaterOrEqualTo(1);
            }

            using (var fc = SampleBootStrapper.GetExport<IDbRepositoryFactory>())
            {
                var ps = fc.For<PersonDb>().AsQueryable()
                    .Include(a => a.Addresses)
                    .Include(a => a.EmailAddresses)
                    .Include(a => a.PhoneNumbers)
                    .First(p => p.Id == id);

                ps.FirstName.Should().Be("Duy");
                ps.LastName.Should().Be("Hoang");
                ps.Addresses.First().BlockNo.Should().Be("123");
                ps.EmailAddresses.First().Email.Should().Be("abc@123.com");
                ps.PhoneNumbers.First().Name.Should().Be("Mobile");
            }
        }

        [TestMethod]
        public void Delete_Contact_With_Address_Phone_Email()
        {
            int id = 0;

            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contact = service.CreateNew("Duy", "Hoang")
                    .WithAddress(new Address("123", "111", string.Empty, string.Empty, string.Empty))
                    .WithEmail(new EmailAddress("Personal", "abc@123.com"))
                    .WithPhone(new PhoneNumber("Mobile", "123456"));

                var saved = service.AddOrUpdate(contact);
                service.Delete(saved).Should().BeTrue();
            }

            using (var fc = SampleBootStrapper.GetExport<IDbRepositoryFactory>())
            {
                fc.For<PersonDb>().AsQueryable().FirstOrDefault(p => p.Id == id).Should().BeNull();
                fc.For<AddressDb>().AsQueryable().FirstOrDefault(a => a.PersonId == id).Should().BeNull();
                fc.For<EmailAddessDb>().AsQueryable().FirstOrDefault(a => a.PersonId == id).Should().BeNull();
                fc.For<PhoneNumberDb>().AsQueryable().FirstOrDefault(a => a.PersonId == id).Should().BeNull();
            }
        }

        [TestMethod]
        public void Get_Contact_With_Address_Phone_Email()
        {
            int id = 0;

            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contact = service.CreateNew("Duy", "Hoang")
                    .WithAddress(new Address("123", "111", string.Empty, string.Empty, string.Empty))
                    .WithEmail(new EmailAddress("Personal", "abc@123.com"))
                    .WithPhone(new PhoneNumber("Mobile", "123456"));

                var saved = service.AddOrUpdate(contact);
                id = saved.Id;
            }

            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var c = service.GetById(id);

                c.Should().NotBeNull();

                c.FirstName.Should().Be("Duy");
                c.LastName.Should().Be("Hoang");
                c.Addresses.First().BlockNo.Should().Be("123");
                c.Addresses.First().Id.Should().BeGreaterOrEqualTo(1);
                c.Emails.First().Email.Should().Be("abc@123.com");
                c.Emails.First().Id.Should().BeGreaterOrEqualTo(1);
                c.Phones.First().Name.Should().Be("Mobile");
                c.Phones.First().Id.Should().BeGreaterOrEqualTo(1);
            }
        }

        [TestMethod]
        public void Get_Contact_With_InvalidKey()
        {
            int id = 0;

            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contact = service.CreateNew("Duy", "Hoang")
                    .WithAddress(new Address("123", "111", string.Empty, string.Empty, string.Empty))
                    .WithEmail(new EmailAddress("Personal", "abc@123.com"))
                    .WithPhone(new PhoneNumber("Mobile", "123456"));

                service.AddOrUpdate(contact);
            }

            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                Action c = () => service.GetById(id);
                c.ShouldThrow<InvaidKeysException>();
            }
        }

        [TestMethod]
        public void Get_All_Contact()
        {
            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contacts = new[] {
                    service.CreateNew("Duy1", "Hoang1"),
                    service.CreateNew("Duy2", "Hoang2"),
                    service.CreateNew("Duy3", "Hoang3"),
                    service.CreateNew("Duy4", "Hoang4"),
                };

                contacts.ForEach(c => service.AddOrUpdate(c));
            }

            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contacts = service.All();
                contacts.Count.Should().BeGreaterOrEqualTo(4);
            }
        }

        [TestMethod]
        public void Delete_All_Contacts()
        {
            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contacts = service.All();

                contacts.ForEach(c => service.Delete(c));
            }

            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contacts = service.All();
                contacts.Count.Should().Be(0);
            }
        }

        [TestMethod]
        public void Get_Page_Contacts()
        {
            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                if (service.CountAll() < 50)
                {
                    for (var i = 1; i <= 100; i++)
                    {
                        var contact = service.CreateNew("Duy", "Hoang")
                            .WithAddress(new Address("123", "111", string.Empty, string.Empty, string.Empty))
                            .WithEmail(new EmailAddress("Personal", "abc@123.com"))
                            .WithPhone(new PhoneNumber("Mobile", "123456"));

                        service.AddOrUpdate(contact);
                    }
                }
            }

            using (var service = SampleBootStrapper.GetExport<IContactService>())
            {
                var contacts = service.GetPage(1, 10);
                contacts.Items.Count.Should().Be(10);

                contacts.Items.All(c => c.Addresses.Count > 0
                    && c.Emails.Count > 0
                    && c.Phones.Count > 0).Should().BeTrue();
            }
        }
    }
}
