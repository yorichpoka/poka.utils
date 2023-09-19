namespace POKA.Utils.ValueObjects
{
    public record RequestScopeId : BaseObjectId
    {
        protected override string _type => "rqtScp";

        public RequestScopeId(Guid value)
            : base(value)
        {
        }

        public override string ToString() => base.ToString();
    }
}
