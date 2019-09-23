using AutoMapper;
using RiceMill_MVC.BAL;
using RiceMill_MVC.BLL;
using RiceMill_MVC.Models;
using RiceMill_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RiceMill_MVC.Controllers
{
    public class LedgerPostingController : Controller
    {
        // GET: LedgerPosting
        LedgerPostingService postingService = new LedgerPostingService();
        PartyBalanceService partyBalanceService = new PartyBalanceService();

        JournalPostingService journalPostingService = new JournalPostingService();
        AccountGroupService accountGroupService = new AccountGroupService();
        AccountLedgerService accledgerService = new AccountLedgerService();
        CustomerService customerService = new CustomerService();
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult AddExpense()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddIncome()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contra()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Journal()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Withdraw()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CashBook(string fromDate, string toDate)
        {
            DateTime from = Convert.ToDateTime(fromDate).Date;
            DateTime to = Convert.ToDateTime(toDate).Date;
            List<CashBookResponse> result = this.postingService.GetCashBook(from, to);
            return base.Json(result, 0);
        }

        public ActionResult GetExpenses(string fromDate, string toDate)
        {
            DateTime from = Convert.ToDateTime(fromDate);
            DateTime to = Convert.ToDateTime(toDate);
            List<JournalMaster> ledger = this.journalPostingService.GetAll(from, to);
            return base.Json(Mapper.Map<List<JournalMaster>, List<JournalMasterResponse>>(ledger), 0);
        }

        [HttpGet]
        public ActionResult ExpenseList(string fromDate, string toDate)
        {
            DateTime from = Convert.ToDateTime(fromDate);
            DateTime to = Convert.ToDateTime(toDate);
            List<LedgerPosting> ledgerPosting = this.postingService.GetAll(from, to, 6) ?? new List<LedgerPosting>();
            return base.Json(Mapper.Map<List<LedgerPosting>, List<LedgerPostingResponse>>(ledgerPosting), 0);
        }

        public ActionResult GetInvoiceNumber()
        {
            string invoiceNumber = new GlobalClass().GetMaxId("Id", "JournalMaster");
            string xx = invoiceNumber.PadLeft(6, '0');
            return Json(xx, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditLedgerPosting(int ledgerPostingId)
        {
            var posting = postingService.GetById(ledgerPostingId);
            var allPosting = partyBalanceService.GetAll();
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReceiptDelete(int id)
        {
            ActionResult actionResult;
            try
            {
                LedgerPosting postingObj = this.postingService.GetById(new int?(id));
                foreach (LedgerPosting item in this.postingService.GetAll(postingObj.VoucherNo, true))
                {
                    item.IsActive = false;
                    this.postingService.Update(item, item.Id);
                }
                PartyBalanceService partyBalanceService = this.partyBalanceService;
                int? ledgerId = postingObj.LedgerId;
                PartyBalance paymentObj = partyBalanceService.GetByVoucher((ledgerId.HasValue ? ledgerId.GetValueOrDefault() : 0), postingObj.VoucherNo);
                this.partyBalanceService.Delete(paymentObj.PartyBalanceId);
                AccountLedger customer = this.accledgerService.GetById(postingObj.LedgerId);
                CustomerService customerService = new CustomerService();
                ledgerId = postingObj.LedgerId;
                rptIndividualLedger_Result due = customerService.GetBalance((ledgerId.HasValue ? ledgerId.GetValueOrDefault() : 0));
                string balanceText = "";
                decimal? balance = due.Balance;
                decimal num = new decimal();
                if ((balance.GetValueOrDefault() < num ? !balance.HasValue : true))
                {
                    balance = due.Balance;
                    balanceText = string.Concat("Balance with Dada Rice Tk=", string.Format("{0:#,#.}", decimal.Round((balance.HasValue ? balance.GetValueOrDefault() : decimal.Zero)), ""), "=");
                }
                else
                {
                    decimal minusOne = decimal.MinusOne;
                    balance = due.Balance;
                    balanceText = string.Concat("Balance with Dada Rice Tk=", string.Format("{0:#,#.}", minusOne * decimal.Round((balance.HasValue ? balance.GetValueOrDefault() : decimal.Zero)), ""), "=");
                }
                SMSEmailService sMSEmailService = new SMSEmailService();
                string[] ledgerName = new string[] { "Dear ", customer.LedgerName, ",Tk=", null, null, null, null, null, null };
                balance = postingObj.Debit;
                ledgerName[3] = string.Format("{0:#,#.}", decimal.Round((balance.HasValue ? balance.GetValueOrDefault() : decimal.Zero)), "");
                ledgerName[4] = "/- Received was wrong posted. Your Ref:No:";
                ledgerName[5] = postingObj.VoucherNo;
                ledgerName[6] = " has been deleted,";
                ledgerName[7] = balanceText;
                ledgerName[8] = " Dada Rice.";
                sMSEmailService.SendOneToOneSingleSms("01739110321", string.Concat(ledgerName));
                actionResult = base.Json("Sucess", 0);
            }
            catch (Exception exception)
            {
                actionResult = base.Json(exception.Message, 0);
            }
            return actionResult;
        }
        public ActionResult PaymentDelete(int id)
        {
            ActionResult actionResult;
            try
            {
                LedgerPosting postingObj = this.postingService.GetById(new int?(id));
                foreach (LedgerPosting item in this.postingService.GetAll(postingObj.VoucherNo, true))
                {
                    item.IsActive = false;
                    this.postingService.Update(item, item.Id);
                }
                PartyBalanceService partyBalanceService = this.partyBalanceService;
                int? ledgerId = postingObj.LedgerId;
                PartyBalance paymentObj = partyBalanceService.GetByVoucher((ledgerId.HasValue ? ledgerId.GetValueOrDefault() : 0), postingObj.VoucherNo);
                this.partyBalanceService.Delete(paymentObj.PartyBalanceId);
                AccountLedger customer = this.accledgerService.GetById(postingObj.LedgerId);
                CustomerService customerService = new CustomerService();
                ledgerId = postingObj.LedgerId;
                rptIndividualLedger_Result due = customerService.GetBalance((ledgerId.HasValue ? ledgerId.GetValueOrDefault() : 0));
                string balanceText = "";
                decimal? balance = due.Balance;
                decimal num = new decimal();
                if ((balance.GetValueOrDefault() < num ? !balance.HasValue : true))
                {
                    balance = due.Balance;
                    balanceText = string.Concat("Balance with Dada Rice Tk=", string.Format("{0:#,#.}", decimal.Round((balance.HasValue ? balance.GetValueOrDefault() : decimal.Zero)), ""), "=");
                }
                else
                {
                    decimal minusOne = decimal.MinusOne;
                    balance = due.Balance;
                    balanceText = string.Concat("Balance with Dada Rice Tk=", string.Format("{0:#,#.}", minusOne * decimal.Round((balance.HasValue ? balance.GetValueOrDefault() : decimal.Zero)), ""), "=");
                }
                SMSEmailService sMSEmailService = new SMSEmailService();
                string[] ledgerName = new string[] { "Dear ", customer.LedgerName, ",Tk=", null, null, null, null, null, null };
                balance = postingObj.Credit;
                ledgerName[3] = string.Format("{0:#,#.}", decimal.Round((balance.HasValue ? balance.GetValueOrDefault() : decimal.Zero)), "");
                ledgerName[4] = " payment was wrong posted. Your Ref No:";
                ledgerName[5] = postingObj.VoucherNo;
                ledgerName[6] = " has been deleted,";
                ledgerName[7] = balanceText;
                ledgerName[8] = " Dada Rice.";
                sMSEmailService.SendOneToOneSingleSms("01739110321", string.Concat(ledgerName));
                actionResult = base.Json("Sucess", 0);
            }
            catch (Exception exception)
            {
                actionResult = base.Json(exception.Message, 0);
            }
            return actionResult;
        }

        public ActionResult GetAllLedgerPosting(DateTime fromDate, DateTime toDate, int id, int VoucherTypeId)
        {
            List<LedgerPosting> ledgerPosting = this.postingService.GetAll(fromDate, toDate, id, VoucherTypeId) ?? new List<LedgerPosting>();
            return base.Json(Mapper.Map<List<LedgerPosting>, List<LedgerPostingResponse>>(ledgerPosting), 0);
        }

        public ActionResult GetContraInvoiceNo()
        {
            string invoice = new GlobalClass().GetMaxId("Id", "LedgerPosting");
            string xx = "JO" + invoice.PadLeft(6, '0');
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddIncomeJournalSave(string voucherNo, int CostHeadId, DateTime voucherDate, string notes, List<JournalDetail> journalDetails, decimal TotalAmount, string ChkNo)
        {
            JournalMaster jmaster = new JournalMaster();
            JournalDetail jd = new JournalDetail();

            jmaster.InvoiceNo = voucherNo;
            jmaster.LadgerDate = voucherDate;
            jmaster.Narration = notes;
            jmaster.TotalAmount = TotalAmount;
            journalPostingService.Save(jmaster);

            jd.LedgerId = CostHeadId;
            jd.Credit = TotalAmount;
            jd.Credit = 0;
            jd.ChequeNo = ChkNo;
            jd.ChequeDate = voucherDate;
            journalPostingService.Save(jd);
            foreach (var item in journalDetails)
            {
                JournalDetail jdetails = new JournalDetail();

                jdetails.LedgerId = item.LedgerId;
                jdetails.Credit = 0;
                jdetails.Debit = item.Credit;
                jdetails.ChequeNo = "";
                jdetails.ChequeDate = voucherDate;
                journalPostingService.Save(jdetails);

            }

            //credit
            foreach (var item in journalDetails)
            {
                LedgerPosting lposting = new LedgerPosting();
                lposting.Credit = 0;
                lposting.VoucherNo = voucherNo;
                lposting.LedgerId = item.LedgerId;
                lposting.Debit = item.Debit ?? 0 + item.Credit ?? 0;
                lposting.PostingDate = voucherDate;
                lposting.ChequeDate = DateTime.Now;
                lposting.ChequeNo = "";
                lposting.VoucherTypeId = 4;
                lposting.Extra1 = "Voucher:" + voucherNo + " " + notes;
                postingService.Save(lposting);
            }
            //debit
            LedgerPosting posting = new LedgerPosting();
            posting.ChequeDate = DateTime.Now;
            posting.VoucherNo = voucherNo;
            posting.ChequeNo = "";
            posting.VoucherTypeId = 4;
            posting.LedgerId = CostHeadId;
            posting.PostingDate = voucherDate;
            posting.Credit = TotalAmount;
            posting.Debit = 0;
            posting.Extra1 = "Voucher:" + voucherNo + " " + notes;
            postingService.Save(posting);

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddExpenseJournalSave(string voucherNo, int CostHeadId, DateTime voucherDate, string notes, List<JournalDetail> journalDetails,decimal TotalAmount, string ChkNo)
        {
            JournalMaster jmaster = new JournalMaster();
            JournalDetail jd = new JournalDetail();

            jmaster.InvoiceNo = voucherNo;
            jmaster.LadgerDate = voucherDate;
            jmaster.Narration = notes;
            jmaster.TotalAmount = TotalAmount;
            journalPostingService.Save(jmaster);

            jd.LedgerId = CostHeadId;
            jd.Credit = TotalAmount;
            jd.Credit = 0;
            jd.ChequeNo = ChkNo;
            jd.ChequeDate = voucherDate;
            journalPostingService.Save(jd);
            foreach (var item in journalDetails)
            {
                JournalDetail jdetails = new JournalDetail();

                jdetails.LedgerId = item.LedgerId;
                jdetails.Credit = 0;
                jdetails.Debit = item.Credit;
                jdetails.ChequeNo = "";
                jdetails.ChequeDate = voucherDate;
                journalPostingService.Save(jdetails);

            }

            //credit
            foreach (var item in journalDetails)
            {
                LedgerPosting lposting = new LedgerPosting();
                lposting.Credit = item.Debit ?? 0 + item.Credit ?? 0; ;
                lposting.VoucherNo = voucherNo;
                lposting.LedgerId = item.LedgerId;
                lposting.Debit = 0;
                lposting.PostingDate = voucherDate;
                lposting.ChequeDate = DateTime.Now;
                lposting.ChequeNo = "";
                lposting.VoucherTypeId = 4;
                lposting.Extra1 = "Voucher:" + voucherNo + " " + notes;
                postingService.Save(lposting);
            }
            //debit
            LedgerPosting posting = new LedgerPosting();
            posting.ChequeDate = DateTime.Now;
            posting.VoucherNo = voucherNo;
            posting.ChequeNo = "";
            posting.VoucherTypeId = 4;
            posting.LedgerId = CostHeadId;
            posting.PostingDate = voucherDate;
            posting.Credit = 0;
            posting.Debit = TotalAmount;
            posting.Extra1 = "Voucher:" + voucherNo + " " + notes;
            postingService.Save(posting);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddContraSave(string voucherNo, int CostHeadId, DateTime voucherDate, string notes, List<LedgerPosting> ledgerPostion, string CostChkNo, decimal TotalAmount, string Radio)
        {
            if (Radio == "Debit")
            {
                LedgerPosting lp = new LedgerPosting();
                lp.VoucherNo = voucherNo;
                lp.VoucherTypeId = 6;
                lp.LedgerId = CostHeadId;
                lp.Debit = TotalAmount;
                lp.Credit = 0;
                lp.InvoiceNo = voucherNo;
                lp.ChequeNo = CostChkNo;
                lp.ChequeDate = voucherDate;
                postingService.Save(lp);
                foreach (var item in ledgerPostion)
                {
                    LedgerPosting ledgersave = new LedgerPosting();
                    ledgersave.VoucherTypeId = 6;
                    ledgersave.VoucherNo = voucherNo;
                    ledgersave.LedgerId = item.LedgerId;
                    ledgersave.Debit = 0;
                    ledgersave.Credit = item.Credit;
                    ledgersave.InvoiceNo = voucherNo;
                    ledgersave.ChequeNo = "";
                    ledgersave.ChequeDate = item.ChequeDate;
                    postingService.Save(ledgersave);
                }
            }
            else {
                LedgerPosting lp = new LedgerPosting();
                lp.VoucherNo = voucherNo;
                lp.VoucherTypeId = 6;
                lp.LedgerId = CostHeadId;
                lp.Debit = TotalAmount;
                lp.Credit = 0;
                lp.InvoiceNo = voucherNo;
                lp.ChequeNo = "";
                lp.ChequeDate = voucherDate;
                postingService.Save(lp);
                foreach (var item in ledgerPostion)
                {
                    LedgerPosting ledgersave = new LedgerPosting();
                    ledgersave.VoucherTypeId = 6;
                    ledgersave.VoucherNo = voucherNo;
                    ledgersave.LedgerId = item.LedgerId;
                    ledgersave.Debit = 0;
                    ledgersave.Credit = item.Credit;
                    ledgersave.InvoiceNo = voucherNo;
                    ledgersave.ChequeNo = item.ChequeNo;
                    ledgersave.ChequeDate = item.ChequeDate;
                    postingService.Save(ledgersave);
                }
            }
            
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult JournalSave(string voucherNo, DateTime voucherDate, string notes, List<LedgerPosting> ledgerPostion)
        {
            foreach (var item in ledgerPostion)
            {
                LedgerPosting ledgersave = new LedgerPosting();
                ledgersave.VoucherTypeId = 6;
                ledgersave.VoucherNo = voucherNo;
                ledgersave.LedgerId = item.LedgerId;
                int a = ledgersave.LedgerId??0;
                if (a == 1 || a == 2 || a == 4 || a == 7 || a == 8 || a == 11 || a == 15 || a == 17 || a == 21 || a == 23)
                {
                    ledgersave.Debit = item.Credit;
                    ledgersave.Credit = 0;
                }
                else {
                    ledgersave.Credit = item.Credit;
                    ledgersave.Debit = 0;
                }
                ledgersave.InvoiceNo = voucherNo;
                ledgersave.ChequeNo = item.ChequeNo;
                ledgersave.ChequeDate = item.ChequeDate;
                postingService.Save(ledgersave);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}