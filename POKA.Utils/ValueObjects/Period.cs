namespace POKA.Utils.ValueObjects
{
    public record Period
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public Period(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new ArgumentOutOfRangeException(nameof(to), to.ToString());
            }

            From = from;
            To = to;
        }
    }
}
