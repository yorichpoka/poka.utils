namespace POKA.Utils.ValueObjects
{
    public record UserId : BaseObjectId
    {
        protected override string _type => "usr";

        public UserId(Guid value)
            : base(value)
        {
        }

        public override string ToString() => base.ToString();
    }
}
