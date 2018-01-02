namespace HBD.EntityFramework.Sample.AutoMappers
{
    //internal sealed class ContactProfile : Profile
    //{
    //    public ContactProfile()
    //    {
    //        CreateMap<PersonDb, Contact>()
    //            .ConstructUsing(s => new Contact(s.Id, s.FirstName, s.LastName))
    //            .AfterMap((p, c) =>
    //            {
    //                c.InternalAddresses.AddRange(p.Addresses.To<Address>());
    //                c.InternalPhones.AddRange(p.PhoneNumbers.To<PhoneNumber>());
    //                c.InternalEmails.AddRange(p.EmailAddresses.To<EmailAddress>());
    //            });

    //        CreateMap<Contact, PersonDb>()
    //           .AfterMap((a, b) =>
    //           {
    //               b.Addresses.AddRange(a.Addresses.To<AddressDb>());
    //               b.PhoneNumbers.AddRange(a.Phones.To<PhoneNumberDb>());
    //               b.EmailAddresses.AddRange(a.Emails.To<EmailAddessDb>());
    //           });
    //    }
    //}
}