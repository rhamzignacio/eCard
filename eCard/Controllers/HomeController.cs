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

            List<MotoRequestModel> approved = new List<MotoRequestModel>();

            if (UniversalService.CurrentUser.Type == "USR")
                approved = ECardService.GetAllMotoPerUser("A", out serverResponse);
            else
                approved = ECardService.GetAllMoto("A", out serverResponse);

            return Json(new { error = serverResponse, approved = approved });
        }

        //===========DECLINED MOTO===========
        [HttpPost]
        public JsonResult GetDeclinedPerUser()
        {
            string serverResponse = "";

            List<MotoRequestModel> declined = new List<MotoRequestModel>();

            if (UniversalService.CurrentUser.Type == "USR")
                declined = ECardService.GetAllMotoPerUser("D", out serverResponse);
            else
                declined = ECardService.GetAllMoto("D", out serverResponse);

            return Json(new { error = serverResponse, declined = declined });
        }

        //===========VOIDED MOTO===========
        [HttpPost]
        public JsonResult GetVoidedPerUser()
        {
            string serverResponse = "";

            List<MotoRequestModel> voided = new List<MotoRequestModel>();

            if (UniversalService.CurrentUser.Type == "USR")
                voided = ECardService.GetAllMotoPerUser("V", out serverResponse);
            else
                voided = ECardService.GetAllMoto("V", out serverResponse);

            return Json(new { error = serverResponse, voided = voided });
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

        //===========MOTO REQUEST==============
        [HttpPost]
        public JsonResult SaveMoto(MotoRequestModel moto)
        {
            string serverResponse = "";

            if (moto != null)
            {
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

        //============USER==============
        [HttpPost]
        public JsonResult ChangePassword(ChangePassModel user)
        {
            string serverResponse = "";

            UserService.ChangePassword(user, out serverResponse);

            return Json(serverResponse);
        }
    }
}