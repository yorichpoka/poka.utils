using POKA.Utils.Interfaces;

namespace POKA.Utils.Extensions
{
    public static class GuidExtensions
    {
        public static TObjectId ToObjectId<TObjectId>(this Guid value) where TObjectId : class, IObjectId
        {
            var result = Activator.CreateInstance(typeof(TObjectId), value) as TObjectId;

            return result;
        }
    }
}
