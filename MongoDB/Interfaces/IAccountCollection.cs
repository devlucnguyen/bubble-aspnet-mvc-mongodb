using MongoDB.Entities;
using System.Collections.Generic;

namespace MongoDB.Interfaces
{
    public interface IAccountCollection
    {
        string Insert(Account account);
        IEnumerable<Account> FindAll(bool viewModel = true);
        Account FindByEmail(string email, bool viewModel = true);
        void Update(Account account);
        Account Find(string id, bool viewModel = true);
        List<Account> Search(string email);
    }
}