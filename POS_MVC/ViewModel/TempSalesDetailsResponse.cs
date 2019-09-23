using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.ViewModel
{
    public class TempSalesDetailsResponse
    {
        public decimal Amount
        {
            get;
            set;
        }

        public int BaleQty
        {
            get;
            set;
        }

        public decimal BaleWeight
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public DateTime? CreatedDate
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public bool? IsActive
        {
            get;
            set;
        }

        public string Notes
        {
            get;
            set;
        }

        public virtual ProductResponse Product
        {
            get;
            set;
        }

        public int ProductId
        {
            get;
            set;
        }

        public decimal Rate
        {
            get;
            set;
        }

        public string SalesInvoice
        {
            get;
            set;
        }

        public int SalesMasterId
        {
            get;
            set;
        }

        public decimal TotalQtyInKG
        {
            get;
            set;
        }

        public string UpdatedBy
        {
            get;
            set;
        }

        public string UpdatedDate
        {
            get;
            set;
        }

        public int WarehouseId
        {
            get;
            set;
        }

        public TempSalesDetailsResponse()
        {
        }
    }
}