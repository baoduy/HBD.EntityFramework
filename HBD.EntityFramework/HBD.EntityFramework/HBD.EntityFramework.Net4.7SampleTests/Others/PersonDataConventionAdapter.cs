using HBD.EntityFramework.Core;
using HBD.EntityFramework.Sample.DbEntities;
using HBD.EntityFramework.DbContexts.DbEntities;
using System.ComponentModel.Composition;

namespace HBD.EntityFramework.Test.Others
{
    [Export(typeof(IPreSaveEventRegister<PersonDb>))]
    [Export(typeof(IPreSaveEventRegister))]
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    class PersonDataConventionAdapter : IPreSaveEventRegister<PersonDb>
    {
        public bool IsCalled { get; private set; }

        public void RaiseEvent(IDbRepositoryFactory factory, EntityStatus<PersonDb> entity)
        {
            IsCalled = true;
        }
    }
}
