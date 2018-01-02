namespace HBD.EntityFramework.Sample.ContactManagement
{
    //public sealed class Contact : Aggregates.Aggregate<int>
    //{
    //    internal List<Address> InternalAddresses { get; }
    //    internal List<PhoneNumber> InternalPhones { get; }
    //    internal List<EmailAddress> InternalEmails { get; }

    //    internal Contact(int id, string firstName, string lastName):base(id)
    //    {
    //        FirstName = firstName;
    //        LastName = lastName;
    //        InternalAddresses = new List<Address>();
    //        InternalPhones = new List<PhoneNumber>();
    //        InternalEmails = new List<EmailAddress>();
    //    }

    //    internal Contact(string firstName, string lastName) : this(0, firstName, lastName)
    //    {
    //    }

    //    public string FirstName { get; }
    //    public string LastName { get; }

    //    public IReadOnlyCollection<Address> Addresses => InternalAddresses;
    //    public IReadOnlyCollection<PhoneNumber> Phones => InternalPhones;
    //    public IReadOnlyCollection<EmailAddress> Emails => InternalEmails;

    //    public Contact WithAddress(Address address)
    //    {
    //        InternalAddresses.Add(address);
    //        return this;
    //    }

    //    public Contact WithPhone(PhoneNumber phone)
    //    {
    //        InternalPhones.Add(phone);
    //        return this;
    //    }

    //    public Contact WithEmail(EmailAddress email)
    //    {
    //        InternalEmails.Add(email);
    //        return this;
    //    }

    //    public void RemoveAddress(Address address) => InternalAddresses.Remove(address);

    //    public void RemovePhone(PhoneNumber phone) => InternalPhones.Remove(phone);

    //    public void RemoveEmail(EmailAddress email) => InternalEmails.Remove(email);
    //}
}