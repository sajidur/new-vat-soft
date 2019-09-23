using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.ViewModel
{
    public class CashBookResponse
    {
        public decimal Credit
        {
            get;
            set;
        }

        public decimal Debit
        {
            get;
            set;
        }

        public string Extra1
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public string LedgerName
        {
            get;
            set;
        }

        public string PostDate
        {
            get
            {
                return this.PostingDate.ToString("dd-MM-yyyy");
            }
            set
            {
                this.PostingDate.ToString("dd-MM-yyyy");
            }
        }

        public DateTime PostingDate
        {
            get;
            set;
        }

        public string VoucherNo
        {
            get;
            set;
        }

        public int VoucherTypeId
        {
            get;
            set;
        }

        public string VoucherTypeName
        {
            get;
            set;
        }

        public CashBookResponse()
        {
        }
    }

}