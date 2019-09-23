using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RiceMill_MVC.ViewModel
{
    public class FinancialYearView
    {
        public int SelectedYearId { get; set; }
        public IEnumerable<SelectListItem> FinancialYears { get; set; }
    }
}