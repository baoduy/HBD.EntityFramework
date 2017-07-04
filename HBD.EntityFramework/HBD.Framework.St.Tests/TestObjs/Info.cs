using HBD.EntityFramework.GlobalShare.Core;

namespace HBD.Framework.St.Tests.TestObjs
{
    public class Info : Value<Info>
    {
        public Info(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public int Index { get; }
        public string Name { get; }

        public override bool Equals(Info other)
        {
            if (other == null) return false;
            return this.Index == other.Index && this.Name == other.Name;
        }
    }
}