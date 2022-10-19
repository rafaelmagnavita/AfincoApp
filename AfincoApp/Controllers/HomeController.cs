using AfincoApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AfincoApp.Controllers
{
    [Authorize]
    [Common.SessionExpireFilter]


    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                Common.LogErros(ex.TargetSite.ToString() + ex.Source.ToString() + ex.Message.ToString());
                return View("~/Views/Home/Index.cshtml");
            }
        }

    }
}