using HBD.EntityFramework.Core;
using HBD.EntityFramework.Sample.DbEntities;
using HBD.EntityFramework.TestSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace HBD.EntityFramework.St20Test
{
    [TestClass]
    public class DbDeletingTests
    {
        [TestMethod]
        public void Delete_SubItems()
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

                rs.Delete(ps).Includes(i => i.Addresses);
                fc.Save("Duy");

                Assert.IsNull(rs.AsQueryable().FirstOrDefault(i => i.Id == ps.Id));
            }
        }

        [TestMethod]
        public async Task Delete_SubItems_Async()
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

                await rs.AddAsync(ps);
                await fc.SaveAsync("Duy");

                await (await rs.DeleteAsync(ps)).IncludesAsync(i => i.Addresses);
                await fc.SaveAsync("Duy");

                Assert.IsNull(rs.AsQueryable().FirstOrDefault(i => i.Id == ps.Id));
            }
        }

        [TestMethod]
        public void Delete_ByKeys()
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

                fc.For<AddressDb>().DeleteByKey(ps.Addresses.First().GetKeys());
                rs.DeleteByKey(ps.GetKeys());
                fc.Save("Duy");

                Assert.IsNull(rs.AsQueryable().FirstOrDefault(i => i.Id == ps.Id));
            }
        }

        [TestMethod]
        public async Task Delete_ByKeys_Async()
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
                await fc.SaveAsync("Duy");

                await fc.For<AddressDb>().DeleteByKeyAsync(ps.Addresses.First().GetKeys());
                await rs.DeleteByKeyAsync(ps.GetKeys());
                await fc.SaveAsync("Duy");

                Assert.IsNull(rs.AsQueryable().FirstOrDefault(i => i.Id == ps.Id));
            }
        }
    }
}
