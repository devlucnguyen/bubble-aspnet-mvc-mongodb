using Common.Constants;
using Common.Utilities;
using MongoDB.Entities;
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
        public ActionResult Login()
        {
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
        #endregion
    }
}