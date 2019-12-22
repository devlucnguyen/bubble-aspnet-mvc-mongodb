using MongoDB.Driver;
using System.Configuration;
using System.Linq;

namespace MongoDB.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties
        public IMongoDatabase DBContext { get; private set; }
        private readonly static string[] connectionConfig = ConfigurationManager.ConnectionStrings["MongoDB"].ToString().Split(';');
        private readonly string connectionString = connectionConfig.First();
        private readonly string database = connectionConfig.Last();
        #endregion

        #region Constructors
        public UnitOfWork()
        {
            var dbClient = new MongoClient(connectionString);
            this.DBContext = dbClient.GetDatabase(database);
        }
        #endregion
    }
}