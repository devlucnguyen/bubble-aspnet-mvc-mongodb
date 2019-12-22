using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MongoDB.Entities
{
    public class Friend
    {
        #region Properties
        [BsonId]
        public ObjectId _id { get; set; }
        public string AccountA_id { get; set; }
        public string AccountB_id { get; set; }
        public DateTime RelationshipDate { get; set; }
        #endregion
    }
}