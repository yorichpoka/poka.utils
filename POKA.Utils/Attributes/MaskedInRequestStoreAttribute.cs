namespace POKA.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MaskedInRequestStoreAttribute : Attribute
    {
        private string _maskValue;

        public MaskedInRequestStoreAttribute(string maskValue = "**********")
        {
            _maskValue = maskValue;
        }

        public string MaskValue => this._maskValue;
    }
}
