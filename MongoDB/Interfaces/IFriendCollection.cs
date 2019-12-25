using MongoDB.Entities;
using System.Collections.Generic;

namespace MongoDB.Interfaces
{
    public interface IFriendCollection
    {
        string Insert(Friend friend);
        IEnumerable<Account> FindByAccount(string accountId);
    }
}