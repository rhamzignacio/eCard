using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCard.Services;

namespace eCard.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult AllMoto()
        {
            return View();
        }

        public JsonResult GetApproved(DateTime start, DateTime end)
        {

            var approved = ECardService.GetAllApprovedReport(start, end, out string error);

            return Json(new { approved, error });
        }

        public JsonResult GetAllMoto(DateTime start, DateTime end)
        {
            var moto = ECardService.GetAllMotoReport(start, end, out string error);

            return Json(new { moto, error });

        }
    }
}