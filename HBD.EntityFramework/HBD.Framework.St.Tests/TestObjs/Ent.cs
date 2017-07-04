using HBD.EntityFramework.Entities;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class Ent1 : Entity
    {
        public Ent1() : this(0)
        {
        }

        public Ent1(int id) : base(id)
        {
        }

        public string Name { get; set; }
    }

    public class Ent2 : NamedEntity
    {
        public Ent2() : this(string.Empty)
        {
        }

        public Ent2(string id) : base(id)
        {
        }

        public string Name { get; set; }
    }
}