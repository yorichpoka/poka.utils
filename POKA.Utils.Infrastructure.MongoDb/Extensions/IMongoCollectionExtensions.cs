using System.Linq.Expressions;
using POKA.Utils.Interfaces;
using System.Collections;
using MongoDB.Driver;

namespace POKA.Utils.Extensions
{
    public static class IMongoCollectionExtensions
    {
        public static async Task UpdateAsync<TEntity>(
            this IMongoCollection<TEntity> mongoCollection,
            Expression<Func<TEntity, bool>> predicate,
            TEntity entity,
            IClientSessionHandle? clientSessionHandle = null,
            CancellationToken cancellationToken = default
        )
            where TEntity : class, IEntity
        {
            var trackedChanges = entity.GetChanges();
            var propertiesChangedSize = trackedChanges.Count();
            var index = 0;
            var updateDefinitions = new UpdateDefinition<TEntity>[propertiesChangedSize];

            foreach (var trakedChange in trackedChanges)
            {
                var propertyName = trakedChange.PropertyName;
                var propertyValue = trakedChange.CurrentValue;
                updateDefinitions[index] = Builders<TEntity>.Update.Set(propertyName, propertyValue);
                index++;
            }

            if (clientSessionHandle != null)
            {
                await
                    mongoCollection
                        .UpdateManyAsync(
                            clientSessionHandle,
                            predicate,
                            Builders<TEntity>.Update.Combine(updateDefinitions),
                            cancellationToken: cancellationToken
                        );
            }
            else
            {
                await
                    mongoCollection
                        .UpdateManyAsync(
                            predicate,
                            Builders<TEntity>.Update.Combine(updateDefinitions),
                            cancellationToken: cancellationToken
                        );
            }

        }

        public static async Task UpdateAsync<TEntity>(
            this IMongoCollection<TEntity> mongoCollection,
            IObjectId objectId,
            TEntity entity,
            IClientSessionHandle? clientSessionHandle = null,
            CancellationToken cancellationToken = default
        )
            where TEntity : class, IEntity
        {
            var filter = Builders<TEntity>
                            .Filter
                            .Eq("_id", objectId.Value.ToString());
            var trackedChanges = entity.GetChanges();
            var propertiesChangedSize = trackedChanges.Count();
            var index = 0;
            var updateDefinitions = new UpdateDefinition<TEntity>[propertiesChangedSize];

            foreach (var trackedChange in trackedChanges)
            {
                var propertyName = trackedChange.PropertyName;
                var propertyValue = trackedChange.CurrentValue;

                if (propertyValue is IEnumerable)
                {
                    var doesNoChange = IsSequenceEqual(trackedChange.OriginalValue, trackedChange.CurrentValue);

                    if (doesNoChange)
                    {
                        continue;
                    }
                }

                updateDefinitions[index] = Builders<TEntity>.Update.Set(propertyName, propertyValue);
                index++;
            }

            updateDefinitions = updateDefinitions
                                    .Where(l => l != null)
                                    .ToArray();

            if (clientSessionHandle != null)
            {
                await
                    mongoCollection
                        .UpdateManyAsync(
                            session: clientSessionHandle,
                            filter: filter,
                            update: Builders<TEntity>.Update.Combine(updateDefinitions),
                            cancellationToken: cancellationToken
                        );
            }
            else
            {
                await
                    mongoCollection
                        .UpdateManyAsync(
                            filter: filter,
                            update: Builders<TEntity>.Update.Combine(updateDefinitions),
                            cancellationToken: cancellationToken
                        );
            }
        }

        public static Task CreateAsync<TEntity>(this IMongoCollection<TEntity> mongoCollection, TEntity entity, IClientSessionHandle? clientSessionHandle = null, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity =>
            clientSessionHandle == null
                ? mongoCollection.InsertOneAsync(entity, null, cancellationToken)
                : mongoCollection.InsertOneAsync(clientSessionHandle, entity, null, cancellationToken);

        private static bool IsSequenceEqual(object array1, object array2)
        {
            var genericType = array1.GetType().GetGenericArguments()[0];
            var enumerableType = typeof(Enumerable);
            var sequenceEqualMethod = enumerableType
                                            .GetMethods()
                                            .FirstOrDefault(
                                                l => l.Name == nameof(Enumerable.SequenceEqual) &&
                                                     l.GetParameters().Count() == 2
                                            );
            sequenceEqualMethod = sequenceEqualMethod.MakeGenericMethod(genericType);
            var sequenceEqualMethodResult = sequenceEqualMethod.Invoke(null, new object[] { array1, array2 });

            return (bool)sequenceEqualMethodResult;
        }
    }
}
