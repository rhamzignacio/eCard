using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCard.Services;

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
    }
}