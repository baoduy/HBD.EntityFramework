using HBD.EntityFramework.Core;

namespace HBD.EntityFramework.Services
{
    public class ActionResult<TEntity> : IDto
    {
        public ActionResult(TEntity entity, ActionType type, int effectedItems)
        {
            Entity = entity;
            Type = type;
            EffectedItems = effectedItems;
        }

        public TEntity Entity { get; }
        public ActionType Type { get; }
        public int EffectedItems { get; }
    }

    public enum ActionType
    {
        None = 0, Added = 1, Updated = 2
    }
}
