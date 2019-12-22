using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Entities
{
    public class RemoveConversation
    {
        #region Properties
        [BsonId]
        public ObjectId _id { get; set; }
        public string Conversation_id { get; set; }
        public string Account_id { get; set; }
        public string Message_id { get; set; }
        #endregion
    }
}