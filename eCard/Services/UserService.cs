using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eCard.Models;

namespace eCard.Services
{
    public class UserService
    {
        public static void ChangePassword(ChangePassModel _user, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    var user = db.UserAccount.FirstOrDefault(r => r.ID == UniversalService.CurrentUser.ID);

                    if(user != null)
                    {
                        if (user.Password == _user.CurrentPass)
                        {
                            user.Password = _user.NewPass;

                            db.Entry(user).State = System.Data.Entity.EntityState.Modified;

                            db.SaveChanges();
                        }
                        else
                        {
                            message = "Invalid Password";
                        }
                    }
                }
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }

        public static List<UserAccountModel> GetAll(out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    var query = from u in db.UserAccount
                                select new UserAccountModel
                                {
                                    ID = u.ID,
                                    Department = u.Department,
                                    CreatedBy = u.CreatedBy,
                                    CreatedDate = u.CreatedDate,
                                    Email = u.Email,
                                    Firstname = u.FirstName,
                                    LastName = u.LastName,
                                    MiddleInitial = u.MiddleInitial,
                                    Status = u.Status,
                                    Type = u.Type,
                                    Username = u.Username,
                                };

                    return query.ToList();
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static void Save(UserAccountModel _user, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    if(_user.ID == Guid.Empty || _user.ID == null) // NEW
                    {
                        UserAccount newAccount = new UserAccount
                        {
                            ID = Guid.NewGuid(),
                            Username = _user.Username,
                            Password = _user.Password,
                            Department = _user.Department,
                            Email = _user.Email,
                            FirstName = _user.Firstname,
                            LastName = _user.LastName,
                            MiddleInitial = _user.MiddleInitial,
                            Status = "Y",
                            Type = _user.Type,
                            CreatedBy = UniversalService.CurrentUser.ID,
                            CreatedDate = DateTime.Now
                        };

                        db.Entry(newAccount).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        var user = db.UserAccount.FirstOrDefault(r => r.ID == _user.ID);

                        if(user != null)
                        {
                            user.Username = _user.Username;

                            if (_user.Password != "" && _user.Password != null)
                                user.Password = _user.Password;

                            user.Department = _user.Department;

                            user.Email = _user.Email;

                            user.FirstName = _user.Firstname;

                            user.LastName = _user.LastName;

                            user.MiddleInitial = _user.MiddleInitial;

                            user.Status = _user.Status;

                            user.Type = _user.Type;

                            user.CreatedBy = UniversalService.CurrentUser.ID;

                            user.CreatedDate = DateTime.Now;

                            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
                }
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }
    }
}