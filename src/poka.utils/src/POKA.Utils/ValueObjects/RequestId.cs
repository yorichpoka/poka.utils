namespace POKA.Utils.ValueObjects
{
    public record RequestId : BaseObjectId
    {
        protected override string _type => "rqt";

        public RequestId(Guid value)
            : base(value)
        {
        }

        public override string ToString() => base.ToString();
    }
}
