//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RiceMill_MVC.Models
{
    using System;
    
    public partial class GetPartyPayment_Result
    {
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public int PartyBalanceId { get; set; }
        public string InvoiceNo { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public int LedgerId { get; set; }
        public string LedgerName { get; set; }
        public string extra1 { get; set; }
        public string Phone { get; set; }
    }
}
