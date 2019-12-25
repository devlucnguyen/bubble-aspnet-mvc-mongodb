using Common.Constants;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Interfaces;
using MongoDB.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Collections
{
    public class AccountCollection : BaseCollection, IAccountCollection
    {
        #region Properties
        private IMongoCollection<Account> Collection { get; set; }
        private IGridFsCollection GridFsCollection { get; set; }
        #endregion

        #region Constructors
        public AccountCollection(IUnitOfWork unitOfWork, IGridFsCollection gridFsCollection) : base(unitOfWork)
        {
            this.Collection = unitOfWork.DBContext.GetCollection<Account>(Constant.CONST_ACCOUNT_COLLECTION);
            this.GridFsCollection = gridFsCollection;
        }
        #endregion

        #region Funtions
        public string Insert(Account account)
        {
            Collection.InsertOne(account);

            return account._id.ToString();
        }

        public Account Find(string id, bool viewModel = true)
        {
            var filter = Builders<Account>.Filter.Eq(Constant.CONST_DB_COLUMN_ID, ObjectId.Parse(id));
            var model = Collection.Find(filter).FirstOrDefault();
            var result = this.CreateModel(model, viewModel);

            return result;
        }

        public IEnumerable<Account> FindAll(bool viewModel = true)
        {
            var accounts = Collection.Find(new BsonDocument()).ToList();
            var result = accounts.Select(account => this.CreateModel(account, viewModel));

            return result;
        }

        public Account FindByEmail(string email, bool viewModel = true)
        {
            var model = Collection.Find(account => account.Email.Equals(email)).FirstOrDefault();
            var result = this.CreateModel(model, viewModel);

            return result;
        }

        public void Update(Account account)
        {
            var filter = Builders<Account>.Filter.Eq(Constant.CONST_DB_COLUMN_ID, account._id);

            Collection.ReplaceOne(filter, account);
        }
        #endregion

        #region Private Functions
        private Account CreateModel(Account account, bool viewModel)
        {
            if(viewModel == true && account != null && string.IsNullOrEmpty(account.Avatar) == false)
            {
                account.Avatar = this.GridFsCollection.DownloadImageURL(account.Avatar);
            }

            return account;
        }
        #endregion
    }
}