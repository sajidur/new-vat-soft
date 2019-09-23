using RiceMill_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.ViewModel
{
    public class AccountLedgerResponse
    {
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

    }
}