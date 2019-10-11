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
using REX_MVC.ViewModel;
using REX_MVC.Util;

namespace REX_MVC.Controllers
{
    public class SuppliersController : Controller
    {
        private SupplierService db = new SupplierService();
        private AccountLedgerService Accounts = new AccountLedgerService();
        private PartyBalanceService partyBalanceService = new PartyBalanceService();
        private LedgerPostingService postingService = new LedgerPostingService();
        FinancialYearService year = new FinancialYearService();

        public ActionResult Index()
        {

            ViewBag.Title = "Supplier";
            return View(new Supplier());
        }
        public ActionResult Transaction()
        {
            return View();
        }
        public ActionResult SupplierLedger()
        {
            return View();
        }

        public ActionResult SupplierDueSummary()
        {
            var duesummary = partyBalanceService.GetDueSummary(2);
            return Json(duesummary, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAll()
        {
            List<Supplier> oSupplier = db.GetAll();
            if (oSupplier == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<Supplier>, List<SupplierResponse>>(oSupplier);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllRiceSupplier()
        {
            List<Supplier> oSupplier = db.GetAllRiceSupplier();
            if (oSupplier == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<Supplier>, List<SupplierResponse>>(oSupplier);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllPaddySupplier()
        {
            List<Supplier> oSupplier = db.GetAllPaddySupplier();
            if (oSupplier == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<Supplier>, List<SupplierResponse>>(oSupplier);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: /Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Supplier oSupplier = db.GetById(id);
            if (oSupplier == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<Supplier, SupplierResponse>(oSupplier);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupplierDueSummaryById(int ledgerId, string fromDate, string toDate)
        {
            //DateTime cFromDate = Convert.ToDateTime(fromDate).Date;
            //DateTime cToDate = Convert.ToDateTime(toDate).Date;
            List<rptCustomerLedger_Result> duesummary = this.partyBalanceService.individualLedger(2, ledgerId, fromDate, toDate);
            var result = AutoMapper.Mapper.Map<List<rptCustomerLedger_Result>, List<CustomerLedgerResultResponse>>(duesummary);
            return base.Json(result, 0);
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
        public ActionResult Create(Supplier supplier, decimal OpeningBalance, string CrOrDr, int create)
        {
            try
            {
                var result = supplier;
                if (ModelState.IsValid)
                {
                    supplier.CreationDate = DateTime.Now;
                    supplier.UpdateDate = DateTime.Now;
                    supplier.Creator = CurrentSession.GetCurrentSession().UserName;
                    supplier.IsActive = true;
                    AccountLedger ledger = new AccountLedger();

                    var accountGroup = new AccountGroupService().GetById(22); //sundry creditor
                    ledger.AccountGroupId = 22;
                    ledger.Address = supplier.Address;
                    ledger.BankAccountNumber = "";
                    ledger.BillByBill = true;
                    ledger.BranchCode = "";
                    ledger.BranchName = "";
                    ledger.CreditLimit = 0.0m;
                    ledger.CreditPeriod = 1;
                    ledger.CrOrDr = Accounts.CheckDrOrCr(accountGroup.Nature);
                    ledger.Email = supplier.Email;
                    ledger.IsDefault = false;
                    ledger.LedgerName = supplier.Name;
                    ledger.Extra2 = supplier.Code;
                    ledger.Mobile = supplier.ContactPersonPhone;
                    ledger.Phone = supplier.Phone;
                    ledger.OpeningBalance = OpeningBalance;
                    var saved = Accounts.Save(ledger);
                    supplier.LedgerId = saved.Id;
                    result = db.Save(supplier);
                    if (ledger.OpeningBalance > 0.0m)
                    {
                        var party = new PartyBalance();
                        LedgerPosting post = new LedgerPosting();
                        post.InvoiceNo = "";
                        post.LedgerId = supplier.LedgerId;
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
                        party.LedgerId = supplier.LedgerId ?? 0;
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
            catch(Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
          
        }

        [HttpPost]
        public ActionResult Edit(Supplier model, decimal OpeningBalance, string CrOrDr, int create)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.GetById(model.Id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            supplier.Address = model.Address;
            supplier.City = model.City;
            supplier.Code = model.Code;
            supplier.Name = model.Name;
            supplier.Phone = model.Phone;
            supplier.ContactPersonName = model.ContactPersonName;
            supplier.ContactPersonPhone = model.ContactPersonPhone;
            supplier.Email = model.Email;
            supplier.IsActive = true;
            supplier.UpdateDate = DateTime.Now;
            supplier.UpdateBy = CurrentSession.GetCurrentSession().UserName;
            db.Update(supplier, model.Id);

            //account ledger update
            AccountLedger ledger = new AccountLedgerService().GetById(supplier.LedgerId);
            ledger.Address = supplier.Address;
            ledger.BankAccountNumber = "";
            ledger.BillByBill = true;
            ledger.BranchCode = "";
            ledger.BranchName = "";
            ledger.CreditLimit = 0.0m;
            ledger.CreditPeriod = 1;
            ledger.Email = supplier.Email;
            ledger.IsDefault = false;
            ledger.LedgerName = supplier.Name;

            ledger.Extra2 = supplier.Code;
            ledger.Mobile = supplier.ContactPersonPhone;
            ledger.Phone = supplier.Phone;
            ledger.OpeningBalance = OpeningBalance;
            var saved = Accounts.Update(ledger,supplier.LedgerId??0);
            if (OpeningBalance > 0.0m)
            {
                var party = new PartyBalanceService().GetAll().Where(a=>a.VoucherTypeId==1&&a.LedgerId==supplier.LedgerId).FirstOrDefault();
                LedgerPosting post = new LedgerPostingService().GetAll().Where(a=>a.VoucherTypeId==1 && a.LedgerId == supplier.LedgerId).FirstOrDefault();
                if (post == null)
                {
                    if (party==null)
                    {
                        party = new PartyBalance();
                    }
                    post = new LedgerPosting();
                    post.LedgerId = supplier.LedgerId;
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
                if (party==null||party.PartyBalanceId==0)
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
                    party.LedgerId = supplier.LedgerId ?? 0;
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
                    partyBalanceService.Update(party,party.PartyBalanceId);

                }

           
            }
            return Json("Updated", JsonRequestBehavior.AllowGet);
            //return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.GetById(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            supplier.IsActive = false;
            db.Update(supplier, id ?? 0);
            //int delete = db.Delete(id ?? 0);
            return Json("Deleted", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SingleSupplierLedger(int ledgerId)
        {
            return View(ledgerId);
        }


    }
}
