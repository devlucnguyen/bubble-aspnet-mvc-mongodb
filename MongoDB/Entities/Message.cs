using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MongoDB.Entities
{
    public class Message
    {
        #region Properties
        [BsonId]
        public ObjectId _id { get; set; }
        public string Conversation_id { get; set; }
        public string AccountSend_id { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public DateTime SendDate { get; set; }
        #endregion
    }
}