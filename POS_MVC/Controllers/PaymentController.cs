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
    public class PaymentController : Controller
    {
        // GET: Payment
        PaymentService payment = new PaymentService();
        AccountLedgerService ledgerService = new AccountLedgerService();
        LedgerPostingService postingService = new LedgerPostingService();
        PartyBalanceService partyBalanceService = new PartyBalanceService();
        SupplierService supplierService = new SupplierService();
        CustomerService customerService = new CustomerService();

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LocalPayment()
        {
            return View();
        }
        public ActionResult Transfer()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoadAllLocalPayment(DateTime fromDate,DateTime toDate)
        {
            List<LocalMarketPayment> paymentList = new List<LocalMarketPayment>();
            var partyPayment=  partyBalanceService.GetAll(fromDate, toDate).OrderBy(a=>a.InvoiceNo);
            foreach (var item in partyPayment)
            {
                LocalMarketPayment payment = new LocalMarketPayment();              
                var invoice= paymentList.Where(a => a.InvoiceNo == item.AgainstInvoiceNo).FirstOrDefault();
                if (invoice==null)
                {
                    payment.LedgerId = item.LedgerId;
                    if (string.IsNullOrEmpty(item.InvoiceNo))
                    {
                        payment.InvoiceNo = "";
                    }
                    else
                    {
                        payment.InvoiceNo = item.InvoiceNo;
                    }
                    payment.PaidAmount = item.Debit??0;
                    payment.Party = item.AccountLedger.LedgerName;
                    payment.PostingDate = item.PostingDate.Value.ToString("dd-MM-yyyy")??DateTime.Now.ToString("dd-MM-yyyy");
                    payment.TotalAmount = item.Credit??0;
                    payment.RestAmount = payment.TotalAmount - payment.PaidAmount;
                    paymentList.Add(payment);
                }
                else
                {
                    invoice.PaidAmount =invoice.PaidAmount+ item.Credit??0;
                    invoice.RestAmount = payment.TotalAmount - invoice.PaidAmount;
                }

            }
            return Json(paymentList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LocalPayment(List<LocalMarketPayment> payments,LedgerPosting ledger)
        {
            foreach (var item in payments)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadPayment()
        {
            var getAllLedger= ledgerService.GetAll();
            var result = AutoMapper.Mapper.Map<List<AccountLedger>, List<AccountLedgerResponse>>(getAllLedger);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoadPayment(int ledgerId)
        {
            List<LocalMarketPayment> paymentList = new List<LocalMarketPayment>();
            var partyPayment = partyBalanceService.GetAllInvoice(ledgerId,2).OrderBy(a => a.VoucherNo);
            foreach (var item in partyPayment)
            {
                LocalMarketPayment payment = new LocalMarketPayment();
                var invoice = paymentList.Where(a => a.InvoiceNo == item.InvoiceNo).FirstOrDefault();
                if (invoice == null)
                {
                    payment.InvoiceNo = item.InvoiceNo;
                    payment.PaidAmount = item.Credit;
                    payment.Party = item.LedgerName;
                    payment.PostingDate = item.PostingDate?.ToString("dd-MM-yyyy");
                    payment.TotalAmount = item.Debit;
                    payment.RestAmount = payment.TotalAmount - payment.PaidAmount;
                    paymentList.Add(payment);
                }
                else
                {
                    invoice.PaidAmount = invoice.PaidAmount + item.Credit;
                    invoice.RestAmount = payment.TotalAmount - invoice.PaidAmount;
                }

            }
            return Json(paymentList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ReceivePayment(string voucherNo, int supplierId, DateTime voucherDate, string notes, List<LedgerPosting> ledgerPosting,bool isSendSMS)
        {
            decimal? credit;
            var supplierInfo = supplierService.GetById(supplierId);
            if (supplierInfo == null)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            //credit
            foreach (var item in ledgerPosting)
            {
                item.Credit = 0;
                item.VoucherNo = voucherNo;
                item.PostingDate = voucherDate;
                item.ChequeDate = DateTime.Now;
                item.ChequeNo = "";
                item.VoucherTypeId =(int)BAL.VoucherType.PaymentVoucher;
                item.Extra1 = "Voucher:" + voucherNo + " " + notes;
                postingService.Save(item);
            }
            //debit
            LedgerPosting posting = new LedgerPosting();
            posting.ChequeDate = DateTime.Now;
            posting.VoucherNo = voucherNo;
            posting.ChequeNo = "";
            posting.VoucherTypeId = (int)BAL.VoucherType.PaymentVoucher;
            posting.LedgerId = supplierInfo.LedgerId;
            posting.PostingDate = voucherDate;
            posting.Credit = ledgerPosting.Sum(a => a.Debit).Value;
            posting.Debit = 0;
            posting.Extra1 = "Voucher:" + voucherNo + " " + notes;
            postingService.Save(posting);
            //balance
            //party balance 
            PartyBalance balance = new PartyBalance();
            balance.AgainstVoucherTypeId = 4;
            balance.VoucherNo = voucherNo;
            balance.PostingDate = voucherDate;
            balance.LedgerId = supplierInfo.LedgerId ?? 0;
            balance.Debit = posting.Credit;
            balance.Credit = 0;
            balance.VoucherTypeId =(int) BAL.VoucherType.PaymentVoucher;
            balance.extra1 = "Payment Invoice: " + voucherNo + " Notes:" + notes;
            balance.extra2 = posting.Id.ToString();

            partyBalanceService.Save(balance);

            if (isSendSMS)
            {
                SMSEmailService sMSEmailService = new SMSEmailService();
                string phone = supplierInfo.Phone;
                rptIndividualLedger_Result due = customerService.GetBalance((supplierInfo.LedgerId??0));

                string balanceText = "";
                credit = due.Balance;
                var num = new decimal();
                if ((credit.GetValueOrDefault() < num ? !credit.HasValue : true))
                {
                    credit = due.Balance;
                    balanceText = string.Concat("Your Present Balance With Dada Rice Tk=", string.Format("{0:#,#.}", decimal.Round((credit.HasValue ? credit.GetValueOrDefault() : decimal.Zero)), ""), "/=.");
                }
                else
                {
                    decimal minusOne = decimal.MinusOne;
                    credit = due.Balance;
                    balanceText = string.Concat("Your Present Balance With Dada Rice Tk=", string.Format("{0:#,#.}", minusOne * decimal.Round((credit.HasValue ? credit.GetValueOrDefault() : decimal.Zero)), ""), "/= Thanks.");
                }
                balanceText = "";
                string[] name = new string[] { "Dear ", supplierInfo.Name, " Tk=", null, null, null };
                credit = balance.Credit;
                name[3] = string.Format("{0:#,#.}", decimal.Round((posting.Credit.HasValue ? posting.Credit.GetValueOrDefault() : decimal.Zero)), "");
                name[4] = "/- has been Deposited to your Account. Ref:No-"+ voucherNo+", Dated:"+ voucherDate.ToString("dd-MM-yyyy")+". Thanks with Dada Rice";
                sMSEmailService.SendOneToOneSingleSms("01979110321", string.Concat(string.Concat(name), balanceText),false);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInvoiceNumber()
        {
            string invoiceNumber = DateTime.Now.Year +
                new GlobalClass().GetMaxId("Id", "LedgerPosting");
            return Json(invoiceNumber, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllInvoice(string fromDate, string toDate)
        {
          //  DateTime from = Convert.ToDateTime(fromDate);
           // DateTime to = Convert.ToDateTime(toDate);

            var partybalance = partyBalanceService.DailyReceiveAndPayment(2,fromDate,toDate);
            return Json(partybalance, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadTodayPayment(DateTime clientDate)
        {
            var totalPayment = postingService.GetTodayPayment(clientDate);
            return Json(totalPayment, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LocalPaymentSave(decimal TotalAmount,List<PartyBalance> partyBalanceList, List<LedgerPosting>ledgerPostingList)
        {
            decimal a,b;
            foreach (var item in partyBalanceList)
            {
                PartyBalance pb = new PartyBalance();
                pb.PostingDate = item.PostingDate;
                pb.LedgerId = item.LedgerId;
                pb.VoucherTypeId = 13;
                pb.InvoiceNo = item.InvoiceNo;
                a = item.Debit ?? 0;
                b = item.Credit ?? 0;
                pb.Debit = a + b;
                pb.Credit = item.Balance;
                partyBalanceService.Save(pb);
            }
            
            foreach (var li in ledgerPostingList)
            {
                LedgerPosting lp = new LedgerPosting();
                lp.VoucherNo = li.VoucherNo;
                lp.PostingDate = li.ChequeDate;
                lp.VoucherTypeId = 13;
                lp.LedgerId = li.LedgerId;
                lp.Debit = li.Credit;
                lp.InvoiceNo = "";
                lp.ChequeNo = li.ChequeNo;
                lp.ChequeDate = li.ChequeDate;
                postingService.Save(lp);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}