using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.ViewModel
{
    public class TopSellResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        //added by al ameen
        public int SalesQty { get; set; }
        //public decimal Rate { get; set; }

    }
}