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
    using System.Collections.Generic;
    
    public partial class PartyPaymentHistory
    {
        public int Id { get; set; }
        public Nullable<int> PartyBalanceId { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public Nullable<int> LedgerId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> PaymentCash { get; set; }
        public Nullable<decimal> PaymentChecque { get; set; }
        public Nullable<System.DateTime> ChecqueDate { get; set; }
        public string Bank { get; set; }
        public string Notes { get; set; }
    }
}