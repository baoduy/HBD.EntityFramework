using HBD.EntityFramework.Core;
using HBD.EntityFramework.Sample.DbEntities;
using HBD.EntityFramework.DbContexts.DbEntities;
using System.ComponentModel.Composition;

namespace HBD.EntityFramework.Test.Others
{
    [Export(typeof(IDataConventionAdapter<PersonDb>))]
    [Export(typeof(IDataConventionAdapter))]
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    class PersonDataConventionAdapter : IDataConventionAdapter<PersonDb>
    {
        public bool IsCalled { get; private set; }

        public void ApplyFor(EntityStatus<PersonDb> entity)
        {
            IsCalled = true;
        }
    }
}
