﻿using RiceMill_MVC.BAL;
using RiceMill_MVC.Models;
using RiceMill_MVC.Util;
using RiceMill_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RiceMill_MVC.Controllers
{
    public class FinancialYearController : Controller
    {
        FinancialYearService year = new FinancialYearService();
        // GET: FinancialYear
        public ActionResult Index()
        {
            var categories =year.GetAll().ToList();
            var financialyear = CurrentSession.GetCurrentSession();
            var model = new FinancialYearView
            {
                SelectedYearId = financialyear.FinancialYear,
                FinancialYears = categories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Extra1
                })
            };
            return PartialView("_FinancialYear", model);
        }

        public ActionResult ChangeFinancialYear(int newYearId)
        {
            var newSession= CurrentSession.GetCurrentSession();
            newSession.FinancialYear = newYearId;
            Session["Session"] = newSession;
            return Json("CHanged to"+newYearId, JsonRequestBehavior.AllowGet);
        }
    }
}