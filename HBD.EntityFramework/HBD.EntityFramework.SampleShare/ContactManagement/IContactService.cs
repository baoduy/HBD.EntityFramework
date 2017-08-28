using System.Collections.Generic;
using System;
using HBD.EntityFramework.Core;

namespace HBD.EntityFramework.Sample.ContactManagement
{
    public interface IContactService : IDisposable
    {
        IReadOnlyCollection<Contact> All();
        Contact CreateNew(string firstName, string lastName);
        Contact AddOrUpdate(Contact entity);
        bool Delete(Contact entity);
        Contact GetById(int key);
        IPagable<Contact> GetPage(int pageIndex, int pageSize);
        int CountAll();
    }
}