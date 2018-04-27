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

            return Json(new { error = serverResponse, approved });
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

            return Json(new { error = serverResponse, declined });
        }

        [HttpPost]
        public JsonResult ComputeTotal(double? amount, double? serviceFee, double? otherFee, double? adminFee)
        {
           double? total = 0;

            if (amount == null)
                amount = 0;

            if (serviceFee == null)
                serviceFee = 0;

            if (otherFee == null)
                otherFee = 0;

            if (adminFee == null)
                adminFee = 0;

            total = amount + serviceFee + otherFee + adminFee;

            return Json(string.Format("{0:0.00}", total));
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

            return Json(new { error = serverResponse, voided });
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

            return Json(new { error = serverResponse, request });
        }

        //===========MOTO REQUEST==============
        [HttpPost]
        public JsonResult ComputeAdminFee(MotoRequestModel moto)
        {
            double adminFee = AdminFeeFormula.GetAdminFeeFromDB(moto, out string serverResponse);

            if (moto.Amount == null)
                moto.Amount = 0;

            if (moto.BCDFee == null)
                moto.BCDFee = 0;

            if (moto.Others == null)
                moto.Others = 0;

            double? total = double.Parse(moto.Amount.ToString()) + 
                double.Parse(moto.BCDFee.ToString()) +
                double.Parse(moto.Others.ToString()) + adminFee;

            return Json(new { total = String.Format("{0:0.00}", total), adminFee = String.Format("{0:0.00}",adminFee),
                error = serverResponse});
        }

        [HttpPost]
        public JsonResult SaveMoto(MotoRequestModel moto)
        {
            string serverResponse = "";

            if (moto != null)
            {
                if (moto.Status == "F" && UniversalService.CurrentUser.Type == "APR")
                    moto.Status = "V";

                ECardService.SaveMotoRequest(moto, out serverResponse);
            }

            return Json(serverResponse);
        }

        [HttpPost]
        public JsonResult GetDropDowns()
        {
            string serverResponse = "";

            var clientDropDown = QuickipediaService.GetClientDropDown(out serverResponse);

            return Json(new { error = serverResponse, clientDropDown });
        }

        [HttpPost]
        public JsonResult GetDuplicate(MotoRequestModel moto)
        {
            string serverResponse = "";

            if(moto != null)
            {
                var duplicate = ECardService.GetDuplicate(moto, out serverResponse);

                return Json(new { error = serverResponse, duplicate });
            }

            return Json(serverResponse);
        }

        //==========LOGIN=============
        [HttpPost]
        public JsonResult TryLogin(string _username, string _password)
        {

            var user = LoginService.ValidateLogin(_username, _password, out string serverResponse);

            if (user != null)
            {
                LoginService.LoginToSession(user);
            }

            return Json(serverResponse);
        }

        [HttpPost]
        public JsonResult Logout()
        {

            LoginService.LogoutFromSession(out string serverResponse);

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

        [HttpPost]
        public JsonResult Search(string search)
        {
            var SearchItems =  ECardService.Search(search, out string error);

            return Json(new { SearchItems, error });
        }
    }
} 