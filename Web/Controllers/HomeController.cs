using MongoDB.Entities;
using MongoDB.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Web.App_Start;
using Web.Hubs;
using Web.Models;
using Web.WebUtilities;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        #region Properties
        private IAccountCollection AccountCollection { get; set; }
        private IFriendCollection FriendCollection { get; set; }
        private IConversationCollection ConversationCollection { get; set; }
        private IMessageCollection MessageCollection { get; set; }
        #endregion

        #region Contructor
        public HomeController(IAccountCollection accountCollection, IFriendCollection friendCollection,
            IConversationCollection conversationCollection, IMessageCollection messageCollection)
        {
            this.AccountCollection = accountCollection;
            this.FriendCollection = friendCollection;
            this.ConversationCollection = conversationCollection;
            this.MessageCollection = messageCollection;
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            var loggedUser = SessionUtility.GetLoggedUser();
            var friendList = this.FriendCollection.FindByAccount(loggedUser._id.ToString());
            var friendModelList = this.FriendCollection.FindFriendByAccount(loggedUser._id.ToString());
            var conversationList = new List<ConversationModel>();

            foreach (var friend in friendModelList)
            {
                var conversation = this.ConversationCollection.FindByFriend(friend._id.ToString());
                var friendId = friend.AccountA_id == loggedUser._id.ToString() ? friend.AccountB_id : friend.AccountA_id;
                var currentFriend = friendList.Where(user => user._id.ToString().Equals(friendId)).FirstOrDefault();

                if(conversation != null)
                    conversationList.Add(new ConversationModel
                    {
                        _id = conversation._id.ToString(), Friend =  currentFriend,
                        LastMessage = this.MessageCollection.FindByConversation(conversation._id.ToString()).Last(),
                        UnreadMessageList = this.MessageCollection.FindUnreadMessage(conversation._id.ToString())
                    });
            }

            ViewData["friendList"] = friendList;
            ViewData["conversationList"] = conversationList;

            return View();
        }

        [AjaxOnly]
        [HttpPost]
        public JsonResult GetConversation(string friendId)
        {
            var result = new JsonResult { ContentType = "text" };
            var loggedUser = SessionUtility.GetLoggedUser();
            var currentFriend = this.FriendCollection.FindByFriend(loggedUser._id.ToString(), friendId);
            var messageList = new List<Message>();

            if(currentFriend != null)
            {
                var currentconversation = this.ConversationCollection.FindByFriend(currentFriend._id.ToString());

                if (currentconversation == null) //Create new conversation
                {
                    var conversation = new Conversation
                    {
                        Friend_id = currentFriend._id.ToString()
                    };

                    this.ConversationCollection.Insert(conversation);
                }
                else messageList = this.MessageCollection.FindByConversation(currentconversation._id.ToString());

                result.Data = new { type = "success", result = RenderViewToString(this.ControllerContext, "_MessagePartial", messageList) };
            }
            else result.Data = new { type = "error" };

            return result;
        }

        [AjaxOnly]
        [HttpPost]
        public JsonResult SendMessage(string toUserId, string message)
        {
            var result = new JsonResult { ContentType = "text" };
            var loggedUser = SessionUtility.GetLoggedUser();
            var currentFriend = this.FriendCollection.FindByFriend(loggedUser._id.ToString(), toUserId);

            if (currentFriend != null)
            {
                var userOnline = BubbleHub.GetAllConnectedUser();
                var toUserIsOnline = userOnline.Count(user => user.AccountId == toUserId) != 0;
                var conversation = this.ConversationCollection.FindByFriend(currentFriend._id.ToString());
                var newMessage = new Message
                {
                    Conversation_id = conversation._id.ToString(),
                    AccountSend_id = loggedUser._id.ToString(),
                    Content = message,
                    FileName = null,
                    SendDate = DateTime.Now,
                    Status = toUserIsOnline
                };

                this.MessageCollection.Insert(newMessage);
                result.Data = new { type = "success" };
            }
            else result.Data = new { type = "error" };

            return result;
        }

        [AjaxOnly]
        [HttpPost]
        public JsonResult UpdateUnreadMessage(string conversationId)
        {
            this.MessageCollection.UpdateUnreadMessage(conversationId);

            return new JsonResult { ContentType = "text" };
        }
        #endregion

        #region Private Functions
        private string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        #endregion
    }
}