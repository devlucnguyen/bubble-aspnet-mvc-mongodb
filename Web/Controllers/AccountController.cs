using Common.Constants;
using Common.Utilities;
using MongoDB.Entities;
using MongoDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.App_Start;
using Web.Hubs;
using Web.WebUtilities;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        #region Properties
        private IAccountCollection AccountCollection { get; set; }
        private IGridFsCollection GridFsCollection { get; set; }
        private IFriendCollection FriendCollection { get; set; }
        #endregion

        #region Contructor
        public AccountController(IAccountCollection accountCollection, IGridFsCollection gridFsCollection,
            IFriendCollection friendCollection)
        {
            this.AccountCollection = accountCollection;
            this.GridFsCollection = gridFsCollection;
            this.FriendCollection = friendCollection;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Login()
        {
            if (SessionUtility.IsSessionAlive())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Login(Account model)
        {
            var currentAccount = this.AccountCollection.FindByEmail(model.Email);

            if(currentAccount != null && EncryptionUtility.BcryptCheckPassword(model.Password, currentAccount.Password))
            {
                if (currentAccount.Status)
                {
                    var account = this.AccountCollection.Find(currentAccount._id.ToString(), false);
                    var lastLogin = DateTime.Now;
                    account.LastLogin = lastLogin;
                    currentAccount.LastLogin = lastLogin;

                    SessionUtility.SetAuthenticationToken(currentAccount, Constant.CONST_SESSION_TIMEOUT);
                    this.AccountCollection.Update(account);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    model.FirstName = Constant.CONST_MESSAGE_LOGIN_DISABLE;
                    return View(model);
                }
            }

            model.FirstName = Constant.CONST_MESSAGE_LOGIN_INVALID;

            return View(model);
        }

        [HttpPost]
        public ActionResult Register(Account model)
        {
            if(string.IsNullOrEmpty(model.Password) == false)
                model.Password = EncryptionUtility.BcryptHashPassword(model.Password);

            model.ConfirmEmail = false;
            model.CreateDate = DateTime.Now;
            model.Status = true;

            this.AccountCollection.Insert(model);

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            SessionUtility.Logout();

            return RedirectToAction("Login");
        }

        [AjaxOnly]
        [HttpPost]
        public JsonResult GetFriendOnline()
        {
            var result = new JsonResult { ContentType = "text" };
            var connectedUsers = BubbleHub.GetAllConnectedUser();
            var loggedUser = SessionUtility.GetLoggedUser();
            var friendList = FriendCollection.FindByAccount(loggedUser._id.ToString());
            var onlineUser = from friend in friendList
                             join user in connectedUsers on friend._id.ToString() equals user.AccountId
                             select friend._id.ToString();

            return new JsonResult { ContentType = "text", Data = new { user = onlineUser } };
        }

        [AjaxOnly]
        [HttpPost]
        public JsonResult SearchUser(string email)
        {
            var result = new JsonResult { ContentType = "text" };
            var loggedUser = SessionUtility.GetLoggedUser();
            var userList = this.AccountCollection.Search(email);
            var friendList = this.FriendCollection.FindByAccount(loggedUser._id.ToString());

            if(userList .Count > 0)
            {
                var data = userList.Where(user => user._id != loggedUser._id).Select(user => new {
                    Id = user._id.ToString(),
                    FullName = user.GetFullName(),
                    Photo = user.Avatar,
                    IsFriend = friendList.Where(account => account._id == user._id).Count() > 0
                }).OrderByDescending(user => user.IsFriend);

                result.Data = new { type = Constant.CONST_DATA_AJAX_SUCCESS, user = data };
            }else result.Data = new { type = Constant.CONST_DATA_AJAX_ERROR };

            return result;
        }
        #endregion
    }
}