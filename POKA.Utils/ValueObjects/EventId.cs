namespace POKA.Utils.ValueObjects
{
    public record EventId : BaseObjectId
    {
        protected override string _type => "evt";

        public EventId(Guid value)
            : base(value)
        {
        }

        public override string ToString() => base.ToString();
    }
}
