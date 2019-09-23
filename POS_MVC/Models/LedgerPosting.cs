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
    
    public partial class LedgerPosting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LedgerPosting()
        {
            this.BankReconciliations = new HashSet<BankReconciliation>();
        }
    
        public int Id { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public Nullable<int> VoucherTypeId { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<int> LedgerId { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<int> YearId { get; set; }
        public string InvoiceNo { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public Nullable<System.DateTime> ExtraDate { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public bool IsActive { get; set; }
    
        public virtual AccountLedger AccountLedger { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankReconciliation> BankReconciliations { get; set; }
        public virtual FinancialYear FinancialYear { get; set; }
        public virtual VoucherType VoucherType { get; set; }
    }
}
