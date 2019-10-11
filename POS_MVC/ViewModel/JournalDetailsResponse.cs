using System;

namespace REX_MVC.ViewModel
{
    public class JournalDetailsResponse
    {
        public DateTime? ChequeDate
        {
            get;
            set;
        }

        public string ChequeNo
        {
            get;
            set;
        }

        public decimal? Credit
        {
            get;
            set;
        }

        public decimal? Debit
        {
            get;
            set;
        }

        public decimal? ExchangeRate
        {
            get;
            set;
        }

        public string Extra1
        {
            get;
            set;
        }

        public string Extra2
        {
            get;
            set;
        }

        public DateTime? ExtraDate
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public int? JournalMasterId
        {
            get;
            set;
        }

        public int? LedgerId
        {
            get;
            set;
        }

        public JournalDetailsResponse()
        {
        }
    }

}