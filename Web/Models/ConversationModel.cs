using MongoDB.Entities;
using System.Collections.Generic;

namespace Web.Models
{
    public class ConversationModel
    {
        public string _id { get; set; }
        public Account Friend { get; set; }
        public Message LastMessage { get; set; }
        public List<Message> UnreadMessageList { get; set; }
    }
}