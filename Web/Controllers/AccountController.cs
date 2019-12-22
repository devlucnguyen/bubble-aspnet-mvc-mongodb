using Common.Constants;
using Common.Utilities;
using MongoDB.Interfaces;
using System;
using System.Web.Mvc;
using Web.WebUtilities;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        #region Properties
        private IAccountCollection AccountCollection { get; set; }
        private IGridFsCollection GridFsCollection { get; set; }
        #endregion

        #region Contructor
        public AccountController(IAccountCollection accountCollection, IGridFsCollection gridFsCollection)
        {
            this.AccountCollection = accountCollection;
            this.GridFsCollection = gridFsCollection;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var email = collection.Get("email");
            var password = collection.Get("password");
            var currentAccount = this.AccountCollection.FindByEmail(email);

            if(currentAccount != null && EncryptionUtility.BcryptCheckPassword(password, currentAccount.Password))
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
                else return RedirectToAction("Login", new { msg = Constant.CONST_MESSAGE_LOGIN_DISABLE });
            }

            return RedirectToAction("Login", new { msg = Constant.CONST_MESSAGE_LOGIN_INVALID });
        }
        #endregion
    }
}