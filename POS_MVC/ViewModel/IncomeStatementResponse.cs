using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.ViewModel
{
    public class IncomeStatementResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal Type { get; set; }
    }
}