using HBD.EntityFramework.Core;
using HBD.EntityFramework.Sample.DbEntities;
using System.Composition;
using HBD.EntityFramework.DbContexts.DbEntities;

namespace HBD.EntityFramework.St20Test.Others
{
    [Export(typeof(IPreSaveEventRegister<PersonDb>))]
    [Export(typeof(IPreSaveEventRegister))]
    [Export]
    [Shared]
    public class PersonPreSaveEvent : IPreSaveEventRegister<PersonDb>
    {
        public bool IsCalled { get; set; }

        public void RaiseEvent(IDbRepositoryFactory factory, EntityStatus<PersonDb> entity)
        {
            IsCalled = true;
        }
    }
}
