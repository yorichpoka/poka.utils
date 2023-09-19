namespace POKA.Utils.Enums
{
    public sealed class RequestStatusEnum : BaseEnum<RequestStatusEnum>
    {
        public static readonly RequestStatusEnum Pending = new(0, "Pending");
        public static readonly RequestStatusEnum Success = new(1, "Success");
        public static readonly RequestStatusEnum Fail = new(-1, "Fail");

        public RequestStatusEnum(int value, string name)
            : base(value, name)
        {

        }
    }
}
