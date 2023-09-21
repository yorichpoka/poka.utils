using POKA.Utils.ValueObjects;

namespace POKA.Utils.Infrastructure
{
    public interface ICurrentUserIdProvider
    {
        UserId? Id { get; }
    }
}
