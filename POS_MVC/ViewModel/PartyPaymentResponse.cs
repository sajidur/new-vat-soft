using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.ViewModel
{
    public class PartyPaymentResponse
    {
        public PartyPaymentResponse()
        {

        }
        public string LedgerName { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public int LedgerId { get; set; }
        public string InvoiceNo { get; set; }
        public string VoucherNo { get; set; }
        public DateTime? PostingDate { get; set; }
        public string Date { get { return PostingDate?.ToShortDateString(); } set { PostingDate?.ToShortDateString(); } }
        public string extra1 { get; set; }
    }
}