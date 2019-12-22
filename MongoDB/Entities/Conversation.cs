using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Entities
{
    public class Conversation
    {
        #region Properties
        [BsonId]
        public ObjectId _id { get; set; }
        public string Friend_id { get; set; }
        #endregion
    }
}