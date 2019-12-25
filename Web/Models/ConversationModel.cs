using MongoDB.Entities;

namespace Web.Models
{
    public class ConversationModel
    {
        public Account Friend { get; set; }
        public Message LastMessage { get; set; }
    }
}