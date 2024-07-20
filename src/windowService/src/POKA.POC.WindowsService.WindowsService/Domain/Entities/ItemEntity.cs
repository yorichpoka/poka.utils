using Newtonsoft.Json;
using System;

namespace POKA.POC.WindowsService.WindowsService.Domain.Entities
{
    public class ItemEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public ItemEntity(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public override string ToString()
        {
            var jsonStringValue = JsonConvert.SerializeObject(this);

            return jsonStringValue;
        }
    }
}
