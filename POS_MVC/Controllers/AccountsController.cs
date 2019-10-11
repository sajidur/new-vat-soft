using REX_MVC.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REX_MVC.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IncomeStatement()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetIncomeStatement(DateTime fromDate,DateTime toDate)
        {
            LedgerPostingService ledgerPosting = new LedgerPostingService();
            ledgerPosting.GetIncomeStatement(fromDate,toDate);
            return Json("");
        }
    }
}