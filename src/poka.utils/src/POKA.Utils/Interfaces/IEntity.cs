using static POKA.Utils.ChangeTracker;

namespace POKA.Utils.Interfaces
{
    public interface IEntity
    {
        List<ITrackedChange> GetChanges();
    }
}
