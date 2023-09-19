using POKA.Utils.Infrastructure.MongoDb.DbContexts;
using POKA.Utils.Repositories;
using POKA.Utils.ValueObjects;
using System.Linq.Expressions;
using POKA.Utils.Extensions;
using POKA.Utils.Interfaces;
using POKA.Utils.Entities;
using POKA.Utils.Enums;
using Newtonsoft.Json;
using MongoDB.Driver;
using MediatR;

namespace POKA.Utils.Infrastructure.MongoDb.Repositories
{
    public class MongoDbRequestRepository : IRequestRepository
    {
        private readonly IMongoCollection<RequestEntity> _mongoCollection;
        private IApplicationNameProvider _applicationNameProvider;
        private RequestId? _parentId;
        private RequestScopeId _scopeId;

        public MongoDbRequestRepository(
            EventStoreMongoDatabase database, 
            ICollectionNameProvider collectionNameProvider, 
            IApplicationNameProvider applicationNameProvider
        )
        {
            this._applicationNameProvider = applicationNameProvider;
            this._mongoCollection = database.MongoDatabase.GetCollection<RequestEntity>(collectionNameProvider.GetCollectionName<RequestEntity>());
            _scopeId = BaseObjectId.Create<RequestScopeId>();
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
                id: BaseObjectId.Create<RequestId>(),
                status: RequestStatusEnum.Pending,
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
