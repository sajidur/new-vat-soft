using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.ViewModel
{
    public class CustomerLedgerResultResponse
    {
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public int PartyBalanceId { get; set; }
        public string InvoiceNo { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public int LedgerId { get; set; }
        public string Name { get; set; }
        public string extra1 { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PostDate { get { return PostingDate.Value.ToString("dd-MM-yyyy"); } set { PostingDate.Value.ToString("dd-MM-yyyy"); } }

    }
}