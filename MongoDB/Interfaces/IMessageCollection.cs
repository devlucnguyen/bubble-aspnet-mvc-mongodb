using MongoDB.Entities;
using System.Collections.Generic;

namespace MongoDB.Interfaces
{
    public interface IMessageCollection
    {
        List<Message> FindByConversation(string conversation_id);
        string Insert(Message message);
        List<Message> FindUnreadMessage(string conversationId);
        void UpdateUnreadMessage(string conversationId, Account account);
    }
}