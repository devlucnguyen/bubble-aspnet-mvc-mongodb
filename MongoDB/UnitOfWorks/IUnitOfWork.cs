using MongoDB.Driver;

namespace MongoDB.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IMongoDatabase DBContext { get; }
    }
}