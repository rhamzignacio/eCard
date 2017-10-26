using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eCard.Models;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace eCard.Services
{
    public class LoginService
    {
        public static void LogoutFromSession(out string message)
        {
            try
            {
                message = "";

                FormsAuthentication.SignOut();
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }

        public static UserAccountModel ValidateLogin(string _username, string _password, out string message)
        {
            message = "";

            try
            {
                using(var db = new eCardEntities())
                {
                    var user = db.UserAccount.FirstOrDefault(r => r.Username.ToLower() == _username.ToLower()
                        && r.Password == _password);

                    if (user != null)
                    {
                        if (user.Status == "N")
                            message = "User is deactivated";
                        else
                        {
                            UserAccountModel userModel = new UserAccountModel
                            {
                                ID = user.ID,
                                Username = user.Username,
                                Firstname = user.FirstName,
                                MiddleInitial = user.MiddleInitial,
                                LastName = user.LastName,
                                Department = user.Department,
                                Type = user.Type,
                                Status = user.Type,
                                Email = user.Email
                            };
                            return userModel;
                        }
                    }
                    else
                        message = "Invalid username or password";

                    return null;
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }//Validate Login

        public static bool LoginToSession(UserAccountModel user)
        {
            try
            {
                HttpContext.Current.Session["session_status"] = "online";

                PrincipalSerializeModel serializeModel = new PrincipalSerializeModel();

                serializeModel.Username = user.Username;

                serializeModel.Password = user.Password;

                serializeModel.SessionID = HttpContext.Current.Session.SessionID;

                serializeModel.Status = user.Status;

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string userData = serializer.Serialize(serializeModel);

                FormsAuthenticationTicket authenticationECard = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(30), true, userData);

                string encryptedTicket = FormsAuthentication.Encrypt(authenticationECard);

                HttpCookie authenticationCookie_ECard = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                HttpResponse response = HttpContext.Current.Response;

                response.Cookies.Add(authenticationCookie_ECard);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}