using System.Collections.Generic;
using System.Linq;
using Common.Constants;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Interfaces;
using MongoDB.UnitOfWorks;

namespace MongoDB.Collections
{
    public class FriendCollection : BaseCollection, IFriendCollection
    {
        #region Properties
        private IMongoCollection<Friend> Collection { get; set; }
        private IAccountCollection AccountCollection { get; set; }
        #endregion

        #region Constructors
        public FriendCollection(IUnitOfWork unitOfWork, IAccountCollection accountCollection) : base(unitOfWork)
        {
            this.Collection = unitOfWork.DBContext.GetCollection<Friend>(Constant.CONST_FRIEND_COLLECTION);
            this.AccountCollection = accountCollection;
        }
        #endregion

        #region Functions
        public IEnumerable<Account> FindByAccount(string accountId)
        {
            var friendListA = this.Collection.Find(friend => friend.AccountA_id.Equals(accountId)).ToList();
            var friendListB = this.Collection.Find(friend => friend.AccountB_id.Equals(accountId)).ToList();
            var resultA = friendListA.Select(friend => AccountCollection.Find(friend.AccountB_id));
            var resultB = friendListB.Select(friend => AccountCollection.Find(friend.AccountA_id));
            var result = resultA.Union(resultB);

            return result;
        }

        public string Insert(Friend friend)
        {
            this.Collection.InsertOne(friend);

            return friend._id.ToString();
        }

        public List<Friend> FindFriendByAccount(string accountId)
        {
            var result = this.Collection.Find(friend => friend.AccountA_id.Equals(accountId) 
                || friend.AccountB_id.Equals(accountId)).ToList();

            return result;
        }

        public Friend Find(string id)
        {
            var filter = Builders<Friend>.Filter.Eq(Constant.CONST_DB_COLUMN_ID, ObjectId.Parse(id));
            var result = this.Collection.Find(filter).FirstOrDefault();

            return result;
        }
        public Friend FindByFriend(string accountA_id, string accountB_id)
        {
            var result = this.Collection.Find(friend => (friend.AccountA_id.Equals(accountA_id) && friend.AccountB_id.Equals(accountB_id))
                        || (friend.AccountA_id.Equals(accountB_id) && friend.AccountB_id.Equals(accountA_id))).FirstOrDefault();

            return result;
        }
        #endregion
    }
}