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
    
    public partial class AccountLedger
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccountLedger()
        {
            this.AdvancePayments = new HashSet<AdvancePayment>();
            this.ContraDetails = new HashSet<ContraDetail>();
            this.JournalDetails = new HashSet<JournalDetail>();
            this.LedgerPostings = new HashSet<LedgerPosting>();
            this.PartyBalances = new HashSet<PartyBalance>();
        }
    
        public int Id { get; set; }
        public Nullable<int> AccountGroupId { get; set; }
        public string LedgerName { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public string CrOrDr { get; set; }
        public string Narration { get; set; }
        public string MailingName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Nullable<int> CreditPeriod { get; set; }
        public Nullable<decimal> CreditLimit { get; set; }
        public Nullable<int> PricinglevelId { get; set; }
        public Nullable<bool> BillByBill { get; set; }
        public string Tin { get; set; }
        public Nullable<decimal> RouteId { get; set; }
        public string BankAccountNumber { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public Nullable<System.DateTime> ExtraDate { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public Nullable<int> OrderNo { get; set; }
    
        public virtual AccountGroup AccountGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdvancePayment> AdvancePayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContraDetail> ContraDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JournalDetail> JournalDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LedgerPosting> LedgerPostings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartyBalance> PartyBalances { get; set; }
    }
}