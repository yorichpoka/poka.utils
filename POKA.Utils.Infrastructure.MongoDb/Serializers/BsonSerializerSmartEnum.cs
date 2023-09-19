using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace POKA.Utils.Infrastructure.MongoDb.Serializers
{
    public class BsonSerializerSmartEnum<TEnum> : IBsonSerializer<TEnum>
        where TEnum : BaseEnum<TEnum>
    {
        public Type ValueType => typeof(TEnum);

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value) => GetSerialize(context, args, value);

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEnum value) => GetSerialize(context, args, value);

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) => GetDeserialize(context, args);

        public TEnum Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) => GetDeserialize(context, args);

        public void GetSerialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value is not null)
            {
                var @enum = (TEnum)value;
                context.Writer.WriteString(@enum.Name);
            }
            else
            {
                context.Writer.WriteNull();
            }
        }

        private TEnum GetDeserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var enumValue = default(TEnum);

            if (context.Reader.CurrentBsonType != BsonType.Null)
            {
                var stringEnumValue = context.Reader.ReadString();
                enumValue = BaseEnum<TEnum>.FromValue(stringEnumValue);
            }
            else
            {
                context.Reader.ReadNull();
            }

            return enumValue;
        }
    }
}
