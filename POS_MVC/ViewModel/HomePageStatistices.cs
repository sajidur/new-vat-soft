using System.Collections.Generic;
using REX_MVC.Models;
using REX_MVC.ViewModel;

namespace REX_MVC.Controllers
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