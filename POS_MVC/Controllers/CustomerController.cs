using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using REX_MVC.Models;
using REX_MVC.BAL;
using REX_MVC.Util;
using REX_MVC.ViewModel;

namespace REX_MVC.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerService db = new CustomerService();
        private AccountLedgerService Accounts = new AccountLedgerService();

        private PartyBalanceService partyBalanceService = new PartyBalanceService();
        private LedgerPostingService postingService = new LedgerPostingService();
        FinancialYearService year = new FinancialYearService();

        // GET: /Category/
        public ActionResult Index()
        {
            ViewBag.Title = "Customer";
            return View(new Customer());
        }
        public ActionResult Transaction()
        {
            return View();
        }
        public ActionResult CustomerLedger()
        {
            return View();
        }
        // GET: /Category/Details/5
        public ActionResult GetAll()
        {
            List<Customer> customer = db.GetAll();
            if (customer == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<Customer>, List<CustomerResponse>>(customer);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CustomerDueSummary()
        {
          var duesummary=  partyBalanceService.GetDueSummary(1);
            return Json(duesummary, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CustomerDueSummaryById(int customerid,string fromDate,string toDate)
        {
            //DateTime cFromDate = Convert.ToDateTime(fromDate).Date;
            //DateTime cToDate = Convert.ToDateTime(toDate).Date;
            List<rptCustomerLedger_Result> duesummary = this.partyBalanceService.individualLedger(1, customerid,fromDate,toDate);
            var result = AutoMapper.Mapper.Map<List<rptCustomerLedger_Result>, List<CustomerLedgerResultResponse>>(duesummary);

            return base.Json(result, 0);
        }
        // GET: /Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.GetById(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<Customer, CustomerResponse>(customer);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: /Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Customer customer,decimal OpeningBalance, string CrOrDr, int create)
        {
            var result = customer;
            if (ModelState.IsValid)
            {
                var sesssion = CurrentSession.GetCurrentSession();
                if (sesssion != null)
                {

                }
                customer.CreatedDate = DateTime.Now;
                customer.UpdatedDate = DateTime.Now;
                customer.CreatedBy = CurrentSession.GetCurrentSession().UserName;
                customer.IsActive = true;
                AccountLedger ledger = new AccountLedger();
                var accountGroup = new AccountGroupService().GetById(26); //sundry debitor
                ledger.AccountGroupId = 26;
                ledger.Address = customer.Address;
                ledger.BankAccountNumber = "";
                ledger.BillByBill = true;
                ledger.BranchCode = "";
                ledger.BranchName = "";
                ledger.CreditLimit = 0.0m;
                ledger.CreditPeriod = 1;
                ledger.CrOrDr = Accounts.CheckDrOrCr(accountGroup.Nature);
                ledger.Email = customer.Email;
                ledger.IsDefault = false;
                ledger.LedgerName = customer.Name;
                ledger.Extra2 = customer.Code;
                ledger.Mobile = customer.ContactPersonPhone;
                ledger.Phone = customer.Phone;
                ledger.OpeningBalance = OpeningBalance;
                var saved= Accounts.Save(ledger);
                customer.LedgerId = saved.Id;
                result = db.Save(customer);
                if (ledger.OpeningBalance>0.0m)
                {
                    var party = new PartyBalance();
                    LedgerPosting post = new LedgerPosting();
                    post.InvoiceNo = "";
                    post.LedgerId = customer.LedgerId;
                    post.PostingDate = DateTime.Now;
                    if (CrOrDr == "Cr")
                    {
                        post.Credit = ledger.OpeningBalance;
                        party.Credit = ledger.OpeningBalance;
                    }
                    if (CrOrDr == "Dr")
                    {
                        party.Debit = ledger.OpeningBalance;
                        post.Debit = ledger.OpeningBalance;
                    }
                    post.VoucherTypeId = 1;
                    post.VoucherNo = ledger.Id.ToString();
                    post.InvoiceNo = ledger.Id.ToString();
                    var postingResult = postingService.Save(post);

                    party.AgainstInvoiceNo = postingResult.Id.ToString();
                    party.LedgerId = customer.LedgerId ?? 0;
                    party.CreditPeriod = 60;
                    party.FinancialYearId = CurrentSession.GetCurrentSession().FinancialYear;
                    party.PostingDate = DateTime.Now;
                    party.VoucherTypeId = 1;
                    party.extra1 = "Opening Balance";
                    partyBalanceService.Save(party);

                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Edit(Customer model,decimal OpeningBalance, string CrOrDr, int create)
        {
            Customer customer = db.GetById(model.Id);

            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (customer == null)
            {
                return HttpNotFound();
            }
            customer.Address = model.Address;
            customer.City = model.City;
            customer.Code = model.Code;
            customer.Name = model.Name;
            customer.Phone = model.Phone;
            customer.ContactPersonName = model.ContactPersonName;
            customer.ContactPersonPhone = model.ContactPersonPhone;
            customer.Email = model.Email;
            customer.IsActive = true;
            customer.Limit = model.Limit;
            customer.UpdatedDate = DateTime.Now;
            customer.UpdatedBy = CurrentSession.GetCurrentSession().UserName;
            db.Update(customer, model.Id);

            //account ledger update
            AccountLedger ledger = new AccountLedgerService().GetById(customer.LedgerId);
            ledger.Address = customer.Address;
            ledger.BankAccountNumber = "";
            ledger.BillByBill = true;
            ledger.BranchCode = "";
            ledger.BranchName = "";
            ledger.CreditLimit = 0.0m;
            ledger.CreditPeriod = 1;
            ledger.Email = customer.Email;
            ledger.IsDefault = false;
            ledger.LedgerName = customer.Name;
            ledger.Extra2 = customer.Code;
            ledger.Mobile = customer.ContactPersonPhone;
            ledger.Phone = customer.Phone;
            ledger.OpeningBalance = OpeningBalance;
            var saved = Accounts.Update(ledger, customer.LedgerId ?? 0);
            if (OpeningBalance > 0.0m)
            {
                var party = new PartyBalanceService().GetAll().Where(a => a.VoucherTypeId == 1 && a.LedgerId == customer.LedgerId).FirstOrDefault();
                LedgerPosting post = new LedgerPostingService().GetAll().Where(a => a.VoucherTypeId == 1 && a.LedgerId == customer.LedgerId).FirstOrDefault();
                if (post == null)
                {
                    if (party == null)
                    {
                        party = new PartyBalance();
                    }
                    post = new LedgerPosting();
                    post.LedgerId = customer.LedgerId;
                    post.PostingDate = DateTime.Now;
                    if (CrOrDr == "Cr")
                    {
                        post.Credit = ledger.OpeningBalance;
                        party.Credit = ledger.OpeningBalance;
                    }
                    if (CrOrDr == "Dr")
                    {
                        party.Debit = ledger.OpeningBalance;
                        post.Debit = ledger.OpeningBalance;
                    }
                    post.VoucherTypeId = 1;
                    post.VoucherNo = ledger.Id.ToString();
                    post.InvoiceNo = ledger.Id.ToString();
                    var postingResult = postingService.Save(post);
                    party.AgainstInvoiceNo = postingResult.Id.ToString();
                }
                else
                {
                    if (party == null)
                    {
                        party = new PartyBalance();
                    }
                    if (CrOrDr == "Cr")
                    {
                        post.Credit = ledger.OpeningBalance;
                        party.Credit = ledger.OpeningBalance;
                    }
                    if (CrOrDr == "Dr")
                    {
                        party.Debit = ledger.OpeningBalance;
                        post.Debit = ledger.OpeningBalance;
                    }
                    postingService.Update(post, post.Id);
                }
                if (party == null || party.PartyBalanceId == 0)
                {
                    party = new PartyBalance();
                    if (CrOrDr == "Cr")
                    {
                        post.Credit = ledger.OpeningBalance;
                        party.Credit = ledger.OpeningBalance;
                    }
                    if (CrOrDr == "Dr")
                    {
                        party.Debit = ledger.OpeningBalance;
                        post.Debit = ledger.OpeningBalance;
                    }
                    party.LedgerId = customer.LedgerId ?? 0;
                    party.CreditPeriod = 60;
                    party.FinancialYearId = CurrentSession.GetCurrentSession().FinancialYear;
                    party.PostingDate = DateTime.Now;
                    party.VoucherTypeId = 1;
                    party.extra1 = "Opening Balance";
                    partyBalanceService.Save(party);
                }
                else
                {
                    if (CrOrDr == "Cr")
                    {
                        post.Credit = ledger.OpeningBalance;
                        party.Credit = ledger.OpeningBalance;
                    }
                    if (CrOrDr == "Dr")
                    {
                        party.Debit = ledger.OpeningBalance;
                        post.Debit = ledger.OpeningBalance;
                    }

                    party.PostingDate = DateTime.Now;
                    party.Balance = party.Balance + OpeningBalance;
                    partyBalanceService.Update(party, party.PartyBalanceId);

                }


            }
            return Json("Updated", JsonRequestBehavior.AllowGet);

        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.GetById(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            customer.IsActive = false;
            db.Update(customer, id ?? 0);
            return Json("Deleted", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SingleCustomerLedger(int ledgerId)
        {
            return View(ledgerId);
        }

    }
}
