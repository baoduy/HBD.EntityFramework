using HBD.EntityFramework.GlobalShare.Core;

namespace HBD.EntityFramework.Sample.ContactManagement
{
    public sealed class PhoneNumber : Value<PhoneNumber>
    {
        public PhoneNumber(string name, string phoneNo)
            : this(0, name, phoneNo) { }

        public PhoneNumber(int id, string name, string phoneNo)
        {
            this.Id = id;
            this.Name = name;
            this.PhoneNo = phoneNo;
        }

        public int Id { get; }
        public string Name { get; }
        public string PhoneNo { get; }

        public override bool Equals(PhoneNumber other)
            => PhoneNo == other.PhoneNo;
    }
}