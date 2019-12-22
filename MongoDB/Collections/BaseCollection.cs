using MongoDB.UnitOfWorks;

namespace MongoDB.Collections
{
    public class BaseCollection
    {
        #region Properties
        public IUnitOfWork UnitOfWork { get; set; }
        #endregion

        #region Constructors
        public BaseCollection(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
        #endregion
    }
}