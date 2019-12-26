using Common.Constants;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Interfaces;
using MongoDB.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Collections
{
    public class MessageCollection : BaseCollection, IMessageCollection
    {
        #region Properties
        private IMongoCollection<Message> Collection { get; set; }
        #endregion

        #region Constructors
        public MessageCollection(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.Collection = unitOfWork.DBContext.GetCollection<Message>(Constant.CONST_MESSAGE_COLLECTION);
        }
        #endregion

        #region Function
        public List<Message> FindByConversation(string conversation_id)
        {
            var result = this.Collection.Find(message => message.Conversation_id.Equals(conversation_id))
                .ToList().OrderBy(message => message.SendDate).ToList();

            return result;
        }

        public string Insert(Message message)
        {
            this.Collection.InsertOne(message);

            return message._id.ToString();
        }

        public List<Message> FindUnreadMessage(string conversationId)
        {
            var result = this.Collection.Find(message => message.Conversation_id.Equals(conversationId) && message.Status == false).ToList();

            return result;
        }

        public void UpdateUnreadMessage(string conversationId, Account account)
        {
            var messageList = FindUnreadMessage(conversationId).Where(message => message.AccountSend_id != account._id.ToString()).ToList();

            messageList.ForEach(message => message.Status = true);
            messageList.ForEach(message => Update(message));
        }

        public void Update(Message message)
        {
            var filter = Builders<Message>.Filter.Eq(Constant.CONST_DB_COLUMN_ID, message._id);

            Collection.ReplaceOne(filter, message);
        }
        #endregion
    }
}