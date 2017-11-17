using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCard.Services;
using eCard.Models;

namespace eCard.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string serverResponse = "";

            var users = UserService.GetAll(out serverResponse);

            return Json(new { error = serverResponse, users = users });
        }

        [HttpPost]
        public JsonResult Save(UserAccountModel user)
        {
            string serverResponse = "";

            if (user != null)
            {
                if (IsNull(user.Username))
                    serverResponse = "Username is required";
                else if (IsNull(user.Firstname))
                    serverResponse = "First Name is required";
                else if (IsNull(user.LastName))
                    serverResponse = "Last name is required";
                else if (IsNull(user.Type))
                    serverResponse = "Type is required";
                else
                    UserService.Save(user, out serverResponse);
            }
            else
            {
                serverResponse = "Fill up all required fields";
            }

            return Json(serverResponse);
        }

        private bool IsNull(string _input)
        {
            if (_input == "" || _input == null)
                return true;
            else
                return false;
        }
    }
}