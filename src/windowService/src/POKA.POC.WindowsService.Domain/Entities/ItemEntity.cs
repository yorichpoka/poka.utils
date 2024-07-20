using System;

namespace POKA.POC.WindowsService.Domain.Entities
{
    public class ItemEntity
    {
        public Guid Id { get; private set; }
        public object Value { get; private set; }

        public ItemEntity(object value)
        {
            Id = Guid.NewGuid();
            Value = value;
        }
    }
}
