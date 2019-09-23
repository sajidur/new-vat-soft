using RiceMill_MVC.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RiceMill_MVC.Controllers
{
    public class SMSController : Controller
    {
        // GET: SMS
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SendSMS(bool isCustomer, bool isSupplier,bool isEmployee,string numberList)
        {
            var customer =string.Join(",", new CustomerService().GetAll().Select(a=>a.Phone).ToList());
            string phonenumber= customer +","+ numberList;
            new SMSEmailService().SendOneToManyBulkSms(phonenumber,"","");
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}