using HBD.EntityFramework.Core;
using HBD.EntityFramework.Sample.DbEntities;
using System;
using System.Composition;
using HBD.EntityFramework.DbContexts.DbEntities;

namespace HBD.EntityFramework.St20Test.Others
{
    [Export(typeof(IDataConventionAdapter<PersonDb>))]
    [Export(typeof(IDataConventionAdapter))]
    [Export]
    [Shared]
    public class PersonDataConventionAdapter : IDataConventionAdapter<PersonDb>
    {
        public bool IsCalled { get; private set; }

        public void ApplyFor(EntityStatus<PersonDb> entity)
        {
            IsCalled = true;
        }
    }
}
