//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace REX_MVC.Models
{
    using System;
    
    public partial class IncomeStatement_Result
    {
        public Nullable<System.DateTime> PostingDate { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public string VoucherNo { get; set; }
        public string AccountGroupName { get; set; }
        public int Id { get; set; }
        public string LedgerName { get; set; }
        public int AccountLedgerId { get; set; }
        public string Extra1 { get; set; }
    }
}
