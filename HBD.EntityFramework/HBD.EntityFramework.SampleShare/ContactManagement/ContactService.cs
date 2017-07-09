using HBD.EntityFramework.DbContexts.DbEntities;
using HBD.EntityFramework.DbContexts.Interfaces;
using HBD.EntityFramework.Repositories;
using HBD.EntityFramework.Sample.DbContexts;
using HBD.EntityFramework.Sample.DbEntities;
using HBD.EntityFramework.Services;
using HBD.Framework;
using System.Collections.Generic;
using System.Linq;

#if NETSTANDARD2_0 || NETSTANDARD1_6
using System.Composition;
using Microsoft.EntityFrameworkCore;
#else
using System.ComponentModel.Composition;
using System.Data.Entity;
#endif

namespace HBD.EntityFramework.Sample.ContactManagement
{
    [Export(typeof(IContactService))]
    public class ContactService : DataService<Contact, int>, IContactService
    {
        [ImportingConstructor]
        public ContactService(SampleRepositoryFactory dbFactory) : base(dbFactory)
        {
        }

        protected IDbRepo<PersonDb> Db => DbFactory.For<PersonDb>();

        protected virtual string CurrentUser => "Duy";

        protected virtual bool IsNew(Contact contact) => contact.Id <= 0;

        private IQueryable<PersonDb> AsQueryable(bool includedAll = true)
        {
            var query =Db.AsQueryable();

            if (includedAll)
                query = query.Include(a => a.Addresses)
                  .Include(a => a.EmailAddresses)
                  .Include(a => a.PhoneNumbers);

            return query;
        }

        public override IReadOnlyCollection<Contact> All()
        {
#if NETSTANDARD2_0 || NETSTANDARD1_6
            //AutoMapper is working fine with EF-core without calling List() before convert.
            return Db.AsQueryable().To<Contact>().ToList();
#else
            //AutoMapper is not working fine with LazyLoad of EF6 so that need to call ToList() before convert.
            return Db.AsQueryable().ToList().To<Contact>().ToList();
#endif
        }

        public Contact CreateNew(string firstName, string lastName)
            => new Contact(firstName, lastName);

        public override bool Delete(Contact entity)
        {
            this.ValidateKeys(entity.Id);
           Db.DeleteByKey(entity.Id);
            return DbFactory.Save("Duy") > 0;
        }

        public override Contact GetById(int key)
        {
            this.ValidateKeys(key);
            var ps = this.AsQueryable().FirstOrDefault(p => p.Id == key);
            return ps?.To<Contact>();
        }

        public override IPagable<Contact> GetPage(int pageIndex, int pageSize)
        {
            var query = Db.AsQueryable().AsNoTracking().OrderBy(p => p.FirstName);
            var personPage = query.ToPagable(pageIndex, pageSize);

            //The Includes and Orderby are not working together on EF-Core. So below is the workaround solution to load the children relationship.
            //Should: Find the bester solution in future.
            //Loading the relationship info.
            //Please note that calling AsNoTracking() is important for this situation.

#if NETSTANDARD2_0 || NETSTANDARD1_6
            var ids = personPage.Items.Select(p => p.Id).ToList();

            var addresses = DbFactory.For<AddressDb>().AsQueryable().AsNoTracking().Where(a => ids.Contains(a.PersonId)).ToList();
            var emails = DbFactory.For<EmailAddessDb>().AsQueryable().AsNoTracking().Where(a => ids.Contains(a.PersonId)).ToList();
            var phones = DbFactory.For<PhoneNumberDb>().AsQueryable().AsNoTracking().Where(a => ids.Contains(a.PersonId)).ToList();

            foreach (var ps in personPage.Items)
            {
                ps.Addresses.AddRange(addresses.Where(a => a.PersonId == ps.Id));
                ps.EmailAddresses.AddRange(emails.Where(a => a.PersonId == ps.Id));
                ps.PhoneNumbers.AddRange(phones.Where(a => a.PersonId == ps.Id));
            }
#endif
            //Then convert PersonPage to ContactPage.
            return personPage.ToPage<PersonDb, Contact>();
        }

        protected override ActionResult<Contact> OnAddOrUpdate(Contact entity)
        {
            ActionType type;

            var ps = entity.To<PersonDb>();

            if (IsNew(entity))
            {
                type = ActionType.Added;
               Db.Add(ps);
            }
            else
            {
                type = ActionType.Updated;
                ps.Addresses.ForEach(a => a.PersonId = ps.Id);
                ps.PhoneNumbers.ForEach(a => a.PersonId = ps.Id);
                ps.EmailAddresses.ForEach(a => a.PersonId = ps.Id);

               Db.Update(ps)
                      .Includes(p => p.PhoneNumbers)
                      .Includes(p => p.EmailAddresses)
                      .Includes(p => p.Addresses);
            }

            var result = DbFactory.Save("Duy");
            return new ActionResult<Contact>(ps.To<Contact>(), type, result);
        }

        public override int CountAll() => Db.AsQueryable().Count();
    }
}