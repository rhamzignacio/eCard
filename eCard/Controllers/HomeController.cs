using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCard.Services;
using eCard.Models;

namespace eCard.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        //==========MAIN=========
        [HttpPost]
        public JsonResult GetCurrentUser()
        {
            var user = UniversalService.CurrentUser;

            return Json(user);
        }
        //===========APPROVED MOTO===========
        [HttpPost]
        public JsonResult GetApprovedPerUser()
        {
            string serverResponse = "";

            var approved = ECardService.GetAllMotoPerUser("A", out serverResponse);

            return Json(new { error = serverResponse, approved = approved });
        }

        //===========DECLINED MOTO===========
        [HttpPost]
        public JsonResult GetDeclinedPerUser()
        {
            string serverResponse = "";

            var declined = ECardService.GetAllMotoPerUser("D", out serverResponse);

            return Json(new { error = serverResponse, declined = declined });
        }

        //===========PENDING MOTO============
        [HttpPost]
        public JsonResult GetPending()
        {
            string serverResponse = "";

            List<MotoRequestModel> request = new List<MotoRequestModel>();

            if (UniversalService.CurrentUser.Type == "USR")
                request = ECardService.GetAllMotoPerUser("P", out serverResponse);
            else if (UniversalService.CurrentUser.Type == "APR")
                request = ECardService.GetAllMoto("P", out serverResponse);

            return Json(new { error = serverResponse, request = request });
        }

        [HttpPost]
        public JsonResult CancelMoto(MotoRequestModel moto)
        {
            string serverResponse = "";

            if (moto != null)
            {
                moto.Status = "X"; //Cancel Status

                ECardService.SaveMotoRequest(moto, out serverResponse);
            }

            return Json(serverResponse);
        }

        //===========MOTO REQUEST==============
        [HttpPost]
        public JsonResult RequestMoto(MotoRequestModel moto)
        {
            string serverResponse = "";

            if (moto != null)
            {
                moto.Status = "P"; //Pending Status

                ECardService.SaveMotoRequest(moto, out serverResponse);
            }

            return Json(serverResponse);
        }

        [HttpPost]
        public JsonResult GetDropDowns()
        {
            string serverResponse = "";

            var clientDropDown = QuickipediaService.GetClientDropDown(out serverResponse);

            return Json(new { error = serverResponse, clientDropDown = clientDropDown });
        }

        //==========LOGIN=============
        [HttpPost]
        public JsonResult TryLogin(string _username, string _password)
        {
            string serverResponse = "";

            var user = LoginService.ValidateLogin(_username, _password, out serverResponse);

            if(user != null)
            {
                LoginService.LoginToSession(user);
            }

            return Json(serverResponse);
        }

        [HttpPost]
        public JsonResult Logout()
        {
            string serverResponse = "";

            LoginService.LogoutFromSession(out serverResponse);

            return Json(serverResponse);
        }
    }
}