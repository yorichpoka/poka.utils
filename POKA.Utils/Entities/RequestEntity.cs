using POKA.Utils.ValueObjects;
using POKA.Utils.Interfaces;
using POKA.Utils.Enums;

namespace POKA.Utils.Entities
{
    public class RequestEntity : BaseEntity<RequestId>, IHasCreatedOn
    {
        public string ApplicationPerformer { get; private set; } = null!;
        public RequestStatusEnum Status { get; private set; } = null!;
        public RequestTypeEnum Type { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string Data { get; private set; } = null!;
        public string? Error { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public TimeSpan? Duration { get; private set; }
        public RequestId? ParentId { get; private set; }
        public RequestScopeId? ScopeId { get; private set; }

        private RequestEntity()
        {
        }

        public RequestEntity(RequestId id)
        {
            this.Id = id;
        }

        public RequestEntity(RequestScopeId? scopeId, RequestId id, string applicationPerformer, RequestStatusEnum status, string name, string data, DateTime createdOn, RequestId? parentId = null)
        {
            ApplicationPerformer = applicationPerformer;
            CreatedOn = createdOn;
            ParentId = parentId;
            ScopeId = scopeId;
            Duration = null;
            Status = status;
            Error = null;
            Type = null;
            Name = name;
            Data = data;
            Id = id;
        }

        public void AsCommand()
        {
            this.Type = RequestTypeEnum.Command;
        }

        public void AsQuery()
        {
            this.Type = RequestTypeEnum.Query;
        }

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

        private void Complete()
        {
            this.Duration = DateTime.UtcNow - this.CreatedOn;
        }
    }
}
