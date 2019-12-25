using Common.Constants;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Interfaces;
using MongoDB.UnitOfWorks;

namespace MongoDB.Collections
{
    public class ConversationCollection : BaseCollection, IConversationCollection
    {
        #region Properties
        private IMongoCollection<Conversation> Collection { get; set; }
        #endregion

        #region Constructors
        public ConversationCollection(IUnitOfWork unitOfWork, IAccountCollection accountCollection) : base(unitOfWork)
        {
            this.Collection = unitOfWork.DBContext.GetCollection<Conversation>(Constant.CONST_CONVERSATION_COLLECTION);
        }
        #endregion

        #region Functions
        public Conversation FindByFriend(string friendId)
        {
            var result = this.Collection.Find(conversation => conversation.Friend_id.Equals(friendId)).FirstOrDefault();

            return result;
        }

        public string Insert(Conversation conversation)
        {
            Collection.InsertOne(conversation);

            return conversation._id.ToString();
        }
        #endregion
    }
}