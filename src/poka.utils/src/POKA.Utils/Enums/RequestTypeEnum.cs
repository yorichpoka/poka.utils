namespace POKA.Utils.Enums
{
    public sealed class RequestTypeEnum : BaseEnum<RequestTypeEnum>
    {
        public static readonly RequestTypeEnum Command = new(0, "Command");
        public static readonly RequestTypeEnum Query = new(1, "Query");

        public RequestTypeEnum(int value, string name)
            : base(value, name)
        {

        }
    }
}
