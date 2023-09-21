using POKA.Utils.Infrastructure.MongoDb.DbContexts;
using POKA.Utils.Repositories;
using POKA.Utils.ValueObjects;
using System.Linq.Expressions;
using POKA.Utils.Extensions;
using POKA.Utils.Interfaces;
using POKA.Utils.Entities;
using Newtonsoft.Json;
using MongoDB.Driver;
using MediatR;

namespace POKA.Utils.Infrastructure.MongoDb.Repositories
{
    public class MongoDbRequestRepository : IRequestRepository
    {
        private readonly IMongoCollection<RequestEntity> _mongoCollection;
        private IApplicationNameProvider _applicationNameProvider;
        private ICurrentUserIdProvider _currentUserIdProvider;
        private RequestId? _parentId;
        private RequestScopeId _scopeId;

        public MongoDbRequestRepository(
            EventStoreMongoDatabase database, 
            ICollectionNameProvider collectionNameProvider,
            ICurrentUserIdProvider currentUserIdProvider,
            IApplicationNameProvider applicationNameProvider
        )
        {
            this._applicationNameProvider = applicationNameProvider;
            this._currentUserIdProvider = currentUserIdProvider;
            this._mongoCollection = database.MongoDatabase.GetCollection<RequestEntity>(collectionNameProvider.GetCollectionName<RequestEntity>());
            this._scopeId = BaseObjectId.Create<RequestScopeId>();
        }

        public Task<long> CountAsync(Expression<Func<RequestEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
            this._mongoCollection.CountDocumentsAsync(predicate, cancellationToken: cancellationToken);

        public Task DeleteAsync(Expression<Func<RequestEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
            this._mongoCollection.DeleteManyAsync(predicate, cancellationToken: cancellationToken);

        public Task UpdateAsync(RequestId requestId, RequestEntity request, CancellationToken cancellationToken = default) =>
            this._mongoCollection.UpdateAsync(l => l.Id == requestId, request, null, cancellationToken);

        public async Task<RequestEntity> InitializeAsync(IBaseRequest request, CancellationToken cancellationToken = default)
        {
            var requestEntity = new RequestEntity(
                data: JsonConvert.SerializeObject(request, Constants.DefaultJsonSerializerSettings),
                applicationPerformer: this._applicationNameProvider.ApplicationName,
                createdByUserId: this._currentUserIdProvider.Id,
                name: request.GetType().Name,
                createdOn: DateTime.UtcNow,
                parentId: this._parentId,
                scopeId: this._scopeId
            );

            if (request is IBaseCommand)
            {
                requestEntity.AsCommand();
            }
            else
            {
                requestEntity.AsQuery();
            }

            await this._mongoCollection.CreateAsync(requestEntity, null, cancellationToken);

            requestEntity.BeginChanges();

            return requestEntity;
        }
    }
}
