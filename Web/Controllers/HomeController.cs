using MongoDB.Interfaces;
using System.Linq;
using System.Web.Mvc;
using Web.WebUtilities;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        #region Properties
        private IAccountCollection AccountCollection { get; set; }
        private IFriendCollection FriendCollection { get; set; }
        #endregion
                
        #region Contructor
        public HomeController(IAccountCollection accountCollection, IFriendCollection friendCollection)
        {
            this.AccountCollection = accountCollection;
            this.FriendCollection = friendCollection;
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            var loggedUser = SessionUtility.GetLoggedUser();
            var friendList = this.FriendCollection.FindByAccount(loggedUser._id.ToString());

            ViewData["friendList"] = friendList;

            return View();
        }
        #endregion
    }
}