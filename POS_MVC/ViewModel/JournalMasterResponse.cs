using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REX_MVC.ViewModel
{
    public class JournalMasterResponse
    {
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

        public string InvoiceNo
        {
            get;
            set;
        }

        public JournalDetailsResponse JournalDetails
        {
            get;
            set;
        }

        public DateTime LadgerDate
        {
            get;
            set;
        }

        public string Narration
        {
            get;
            set;
        }

        public string PostingDate
        {
            get
            {
                return this.LadgerDate.ToString("dd-MM-yyyy");
            }
            set
            {
                this.LadgerDate.ToString("dd-MM-yyyy");
            }
        }

        public decimal? SuffixPrefixId
        {
            get;
            set;
        }

        public decimal? TotalAmount
        {
            get;
            set;
        }

        public int? UserId
        {
            get;
            set;
        }

        public string VoucherNo
        {
            get;
            set;
        }

        public int? VoucherTypeId
        {
            get;
            set;
        }

        public int? YearId
        {
            get;
            set;
        }

        public JournalMasterResponse()
        {
        }
    }
}