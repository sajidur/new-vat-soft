using System.Collections.Generic;
using RiceMill_MVC.Models;
using RiceMill_MVC.ViewModel;

namespace RiceMill_MVC.Controllers
{
    public class HomePageStatistices
    {
        public HomePageStatistices()
        {
        }

        public double TodaySales { get; internal set; }
        public List<TopSellResponse> TopSell { get;  set; }
        public int TotalBranch { get; internal set; }
        public decimal TotalInventory { get; internal set; }
    }
}