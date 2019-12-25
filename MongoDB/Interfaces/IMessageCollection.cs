using MongoDB.Entities;
using System.Collections.Generic;

namespace MongoDB.Interfaces
{
    public interface IMessageCollection
    {
        List<Message> FindByConversation(string conversation_id);
    }
}