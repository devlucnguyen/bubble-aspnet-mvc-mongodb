using MongoDB.Entities;

namespace MongoDB.Interfaces
{
    public interface IConversationCollection
    {
        Conversation FindByFriend(string friendId);
        string Insert(Conversation conversation);
    }
}