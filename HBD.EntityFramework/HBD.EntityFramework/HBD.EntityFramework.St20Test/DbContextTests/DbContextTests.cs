using FluentAssertions;
using HBD.EntityFramework.Sample.DbEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HBD.EntityFramework.Core;

using Microsoft.EntityFrameworkCore;
using HBD.EntityFramework.DbContexts;

namespace HBD.EntityFramework.TestSample.DbContextTests
{
    [TestClass]
    public class DbContextTests
    {
        [TestInitialize]
        public void Setup()
        {
            System.IO.File.Delete("SampleDb.db");
        }

        [TestMethod]
        public void Get_DbContext()
        {
            using (var db = SampleBootStrapper.GetExportOrDefault<IDbContext>())
            {
                db.Should().NotBeNull();
            }
        }

        [TestMethod]
        public void Create_NewPerson_Only()
        {
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.EnsureDbCreated();

                var rs = fc.For<PersonDb>();
                var p1 = new PersonDb { FirstName = "Duy", LastName = "Hoang" };
                rs.Add(p1);
                fc.Save("Duy");

                rs.AsQueryable().Should().NotBeEmpty();
                var ps = rs.AsQueryable().First(p => p.Id == p1.Id);

                ps.FirstName.Should().Be("Duy");
                ps.LastName.Should().Be("Hoang");
                ps.CreatedOn.Should().BeOnOrAfter(DateTime.Today);
                ps.CreatedBy.Should().Be("Duy");
                ps.UpdatedBy.Should().BeNullOrEmpty();
                ps.UpdatedOn.Should().BeNull();
                ps.Id.Should().BeGreaterThan(0);
            }
        }

        [TestMethod]
        public void Create_NewPerson_WithAddress()
        {
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.EnsureDbCreated();

                var rs = fc.For<PersonDb>();
                var ps = new PersonDb
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                };
                ps.Addresses.Add(new AddressDb { BlockNo = "2412" });

                rs.Add(ps);
                fc.Save("Duy");

                rs.AsQueryable().Should().NotBeEmpty();
                var ps1 = rs.AsQueryable().First(p => p.Id == ps.Id);

                ps1.Addresses.Should().NotBeEmpty();

                var ad = fc.For<AddressDb>().AsQueryable().First(a => a.PersonId == ps.Id);
                ad.BlockNo.Should().Be("2412");
            }
        }

        [TestMethod]
        public void Verify_LazyLoad_DbContext()
        {
            int id = 0;
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.EnsureDbCreated();

                var rs = fc.For<PersonDb>();
                var ps = new PersonDb
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                };
                ps.Addresses.Add(new AddressDb { BlockNo = "2412" });

                rs.Add(ps);
                fc.Save("Duy");

                ps.Addresses.All(a => a.Id > 0).Should().BeTrue();

                id = ps.Id;
            }

            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                var ps = fc.For<PersonDb>().AsQueryable().First(p => p.Id == id);
                ps.Addresses.Count.Should().BeGreaterOrEqualTo(1);
            }
        }

        [TestMethod]
        public void Add_Update_Address_Via_Deattached_Person()
        {
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.EnsureDbCreated();

                var rs = fc.For<PersonDb>();
                var ps = new PersonDb
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                };

                ps.Addresses.Add(new AddressDb { BlockNo = "2412" });

                rs.Add(ps);
                fc.Save("Duy");

                var ps1 = rs.AsQueryable().Include(o => o.Addresses).AsNoTracking().First(p => p.Id == ps.Id);

                ps1.Addresses.First().BlockNo = "111111";
                ps1.Addresses.Add(new AddressDb { PersonId = ps1.Id, BlockNo = "2222", City = "SG" });
                rs.Update(ps1).Includes(p => p.Addresses);
                fc.Save("Duy");

                var ads = fc.For<AddressDb>().AsQueryable().ToList();
                ads.Any(a => a.BlockNo == "111111").Should().BeTrue();
                ads.Any(a => a.BlockNo == "2222").Should().BeTrue();
            }
        }

        [TestMethod]
        public void Add_Update_Address_Via_NonDeattached_Person()
        {
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.EnsureDbCreated();

                var rs = fc.For<PersonDb>();
                var ps = new PersonDb
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                };

                ps.Addresses.Add(new AddressDb { BlockNo = "2412" });

                rs.Add(ps);
                fc.Save("Duy");

                var ps1 = rs.AsQueryable().Include(o => o.Addresses).First(p => p.Id == ps.Id);

                ps1.Addresses.First().BlockNo = "111111";
                ps1.Addresses.Add(new AddressDb { BlockNo = "2222", City = "SG" });
                rs.Update(ps1).Includes(p => p.Addresses);
                fc.Save("Duy");

                var ads = fc.For<AddressDb>().AsQueryable().ToList();
                ads.Any(a => a.BlockNo == "111111").Should().BeTrue();
                ads.Any(a => a.BlockNo == "2222").Should().BeTrue();
            }
        }

        [TestMethod]
        public void Create_NewPerson_Emails()
        {
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.EnsureDbCreated();

                var rs = fc.For<PersonDb>();
                var ps = new PersonDb
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                };
                ps.EmailAddresses.Add(new EmailAddessDb { Name = "Primary", Email = "abc@drunkcoding.net" });

                rs.Add(ps);
                fc.Save("Duy");

                rs.AsQueryable().Should().NotBeEmpty();
                var ps1 = rs.AsQueryable().First(p => p.Id == ps.Id);

                ps1.EmailAddresses.Should().NotBeEmpty();

                var ad = fc.For<EmailAddessDb>().AsQueryable().First(e => e.PersonId == ps.Id);
                ad.Name.Should().Be("Primary");
                ad.Email.Should().Be("abc@drunkcoding.net");
            }
        }

        [TestMethod]
        public void Create_NewPerson_Emails_NotNullException()
        {
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.EnsureDbCreated();

                var rs = fc.For<PersonDb>();
                var ps = new PersonDb
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                };
                ps.EmailAddresses.Add(new EmailAddessDb { Name = "Primary" });

                rs.Add(ps);

                Action save = () => fc.Save("Duy");
                save.ShouldThrow<ValidationException>("Email field is required");
            }
        }

        [TestMethod]
        public void Validator_Is_Able_Validate_InvalidEmail()
        {
            var email = new EmailAddessDb { Name = "A", Email = "abc" };

            Action a = () => Validator.ValidateObject(email);
            a.ShouldThrow<ValidationException>("Email field is not a valid e-mail address");
        }

        [TestMethod]
        public void Create_NewPerson_Emails_InvalidEmail_Exception()
        {
            using (var fc = SampleBootStrapper.GetExportOrDefault<IDbFactory>())
            {
                fc.EnsureDbCreated();

                var rs = fc.For<PersonDb>();
                var ps = new PersonDb
                {
                    FirstName = "Duy",
                    LastName = "Hoang",
                };
                ps.EmailAddresses.Add(new EmailAddessDb { Name = "Primary", Email = "abc" });

                rs.Add(ps);

                Action save = () => fc.Save("Duy");

                save.ShouldThrow<ValidationException>();
            }
        }
    }
}