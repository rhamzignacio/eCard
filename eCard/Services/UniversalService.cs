using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using eCard.Models;

namespace eCard
{
    public class UniversalService
    {
       public static UserAccountModel CurrentUser
        {
            get
            {
                UserAccountModel user = null;

                HttpCookie authCookie_eCard = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                if(authCookie_eCard != null)
                {
                    FormsAuthenticationTicket authTicket_eCard = FormsAuthentication.Decrypt(authCookie_eCard.Value);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    PrincipalSerializeModel serializeModel = serializer.Deserialize<PrincipalSerializeModel>(authTicket_eCard.UserData);

                    Principal newUser = new Principal(authTicket_eCard.Name);

                    newUser.Username = serializeModel.Username;

                    newUser.SessionID = serializeModel.SessionID;

                    HttpContext.Current.User = newUser;

                    using (var db = new eCardEntities())
                    {
                        var query = from u in db.UserAccount
                                    where u.Username == newUser.Username
                                    select new UserAccountModel
                                    {
                                        ID = u.ID,
                                        Firstname = u.FirstName,
                                        MiddleInitial = u.MiddleInitial,
                                        LastName = u.LastName,
                                        Department = u.Department,
                                        Type = u.Type,
                                        Status = u.Status,
                                        CreatedBy = u.CreatedBy,
                                        CreatedDate = u.CreatedDate,
                                        Email = u.Email
                                    };

                        user = query.FirstOrDefault();
                    }
                }
                return user;
            }
            set { }
        }
    }
}