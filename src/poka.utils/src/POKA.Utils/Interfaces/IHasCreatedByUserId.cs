using POKA.Utils.ValueObjects;

namespace POKA.Utils.Interfaces
{
    public interface IHasCreatedByUserId
    {
        UserId? CreatedByUserId { get; }
    }
}