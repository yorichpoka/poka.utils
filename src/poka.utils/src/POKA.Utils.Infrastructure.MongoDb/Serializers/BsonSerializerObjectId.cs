using MongoDB.Bson.Serialization;
using POKA.Utils.ValueObjects;
using MongoDB.Bson;

namespace POKA.Utils.Infrastructure.MongoDb.Serializers
{
    public class BsonSerializerObjectId<TObjectId> : IBsonSerializer<TObjectId>
        where TObjectId : BaseObjectId
    {
        public Type ValueType => typeof(TObjectId);

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TObjectId value) => GetSerialize(context, args, value);

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value) => GetSerialize(context, args, value);

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) => GetDeserialize(context, args);

        public TObjectId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) => GetDeserialize(context, args);

        private void GetSerialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value is not null)
            {
                var objectId = (BaseObjectId)value;
                context.Writer.WriteString(objectId.Value.ToString());
            }
            else
            {
                context.Writer.WriteNull();
            }
        }

        private TObjectId GetDeserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var objectId = default(TObjectId);

            if (context.Reader.CurrentBsonType != BsonType.Null)
            {
                var stringGuidValue = context.Reader.ReadString();
                var guidValue = Guid.Parse(stringGuidValue);
                objectId = BaseObjectId.Create<TObjectId>(guidValue);
            }
            else
            {
                context.Reader.ReadNull();
            }

            return objectId;
        }
    }

    public class BsonSerializerObjectId : BsonSerializerObjectId<BaseObjectId>
    {

    }
}
