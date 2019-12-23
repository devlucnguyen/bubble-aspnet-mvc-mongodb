namespace Common.Constants
{
    public partial class Constant
    {
        #region DB Column Names
        public static string CONST_DB_COLUMN_ID = "_id";
        #endregion

        #region Session keys
        public static string CONST_SESSION_KEY_LOGGED_USER = "BubbleLoggedUser";
        public static int CONST_SESSION_TIMEOUT = 2;
        #endregion

        #region Messages
        public static string CONST_MESSAGE_EMAIL_SENT_RESET_PASSWORD_SUCCESS = "Email sent with password reset instructions<br/>(Missing Emails! Have you checked your Spam Folder?)";
        public static string CONST_MESSAGE_USERNAME_OR_EMAIL_INVALID = "Username or email does not exist!";
        public static string CONST_MESSAGE_LOGIN_INVALID = "Invalid username or password!";
        public static string CONST_MESSAGE_LOGIN_DISABLE = "Your account has been disabled. Please contact support team Bubble!";
        #endregion
    }
}
