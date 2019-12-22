using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MongoDB.Entities
{
    public class Account
    {
        #region Properties
        [BsonId]
        public ObjectId _id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public bool Gender { get; set; }
        public string Avatar { get; set; }
        public bool ConfirmEmail { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastLogout { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Functions
        public bool IsOnline()
        {
            return this.LastLogin > this.LastLogout;
        }
        #endregion
    }
}