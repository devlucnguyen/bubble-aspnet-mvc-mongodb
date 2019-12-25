using MongoDB.Entities;
using System.Collections.Generic;

namespace MongoDB.Interfaces
{
    public interface IFriendCollection
    {
        string Insert(Friend friend);
        IEnumerable<Account> FindByAccount(string accountId);
        List<Friend> FindFriendByAccount(string accountId);
        Friend Find(string id);
        Friend FindByFriend(string accountA_id, string accountB_id);
    }
}