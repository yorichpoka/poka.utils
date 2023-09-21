using POKA.Utils.ValueObjects;
using POKA.Utils.Interfaces;
using POKA.Utils.Enums;

namespace POKA.Utils.Entities
{
    public class RequestEntity : BaseEntity<RequestId>, IHasCreatedOn, IHasCreatedByUserId
    {
        public string ApplicationPerformer { get; private set; } = null!;
        public RequestStatusEnum Status { get; private set; } = null!;
        public RequestScopeId ScopeId { get; private set; } = null!;
        public RequestTypeEnum Type { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string Data { get; private set; } = null!;
        public DateTime CreatedOn { get; private set; }
        public UserId? CreatedByUserId { get; private set; }
        public RequestId? ParentId { get; private set; }
        public TimeSpan? Duration { get; private set; }
        public string? Error { get; private set; }

        private RequestEntity()
        {
        }

        public RequestEntity(RequestId id)
        {
            Id = id;
        }

        public RequestEntity(
            RequestScopeId scopeId, 
            string applicationPerformer,
            string name, 
            string data, 
            UserId? createdByUserId = null, 
            RequestId? parentId = null
        )
        {
            Id = BaseObjectId.Create<RequestId>();
            Status = RequestStatusEnum.Pending;
            Type = RequestTypeEnum.Query;
            CreatedOn = DateTime.UtcNow;
            ApplicationPerformer = applicationPerformer;
            CreatedByUserId = createdByUserId;
            ParentId = parentId;
            ScopeId = scopeId;
            Duration = null;
            Error = null;
            Name = name;
            Data = data;
        }

        public void AsCommand() => this.Type = RequestTypeEnum.Command;

        public void AsQuery() => this.Type = RequestTypeEnum.Query;

        public void Success()
        {
            this.Status = RequestStatusEnum.Success;
            this.Complete();
        }

        public void Fail(string? error)
        {
            this.Status = RequestStatusEnum.Fail;
            this.Error = error;
            this.Complete();
        }

        private void Complete() => this.Duration = DateTime.UtcNow - this.CreatedOn;
    }
}
