using AutoMapper;
using RiceMill_MVC.BAL;
using RiceMill_MVC.BLL;
using RiceMill_MVC.Models;
using RiceMill_MVC.Util;
using RiceMill_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RiceMill_MVC.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        private InventoryService inventoryService = new InventoryService();
        SalesService salesService = new SalesService();
        LedgerPostingService service = new LedgerPostingService();
        CustomerService customerService = new CustomerService();
        Entities context = new Entities();             
        public ActionResult Index()
        {
            ViewBag.Title = "Sales Report";
            return View();
        }

        public ActionResult Sales()
        {
            ViewBag.Title = "New Sales";
            return View();
        }

        public ActionResult SalesOrder()
        {
            return View();
        }
        public ActionResult SalesOrderList()
        {
            return View();
        }

        public ActionResult Autorized()
        {
            return View();
        }

        public ActionResult SalesReport()
        {
            return View();
        }
        public ActionResult TruckRentReport()
        {
            return View();
        }

        public ActionResult TodaySales(DateTime clientDate)
        {
            var  todaySell = salesService.GetTodaysSales(clientDate);
            return Json(todaySell, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnAutorizedSales()
        {
            var tempList = salesService.GetUnAurhorizedSales();
            var result = Mapper.Map<List<TempSalesMaster>, List<TempSalesMasterResponse>>(tempList);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUnAutorizedSales(int id)
        {
            var tempList = salesService.GetByTempId(id);
            var result = Mapper.Map<TempSalesMaster, TempSalesMasterResponse>(tempList);
            return Json(result,JsonRequestBehavior.AllowGet);
        }


        public ActionResult Authorize(int id,bool isSendSMS)
        {
            var item = salesService.GetByTempId(id);
            decimal? deliveryQty;
            decimal baleQty;
            ActionResult actionResult;
            decimal? nullable;
            string message = "";
            SalesMaster result = new SalesMaster();
            result.SalesInvoice = item.SalesInvoice;
            SalesMaster FinalResult = new SalesMaster();
            FinalResult.SalesInvoice = item.SalesInvoice;
            SalesDetail FinalResultDetail = new SalesDetail();
            SalesOrder resultOrder = new SalesOrder();
            SalesOrder FinalResultOrder = new SalesOrder();
            List<int> lstSalesMasterId = new List<int>();
            try
            {
                result.SalesInvoice = item.SalesInvoice;
                result.SalesOrderId = item.SalesOrderId;
                result.SalesDate = item.SalesDate;
                if (!result.SalesDate.HasValue)
                {
                    result.SalesDate = new DateTime?(DateTime.Now);
                }
                result.SalesBy = CurrentSession.GetCurrentSession().UserName;
                result.CustomerID = item.CustomerID;
                result.AdditionalCost = decimal.Zero;
                result.Discount = item.Discount;
                result.Notes = item.Notes;
                result.Coutha = item.Coutha;
                result.TransportNo = item.TransportNo;
                result.TransportType = item.TransportType;
                result.CreatedBy = CurrentSession.GetCurrentSession().UserName;
                result.CreatedDate = new DateTime?(DateTime.Now);
                result.TotalAmount = item.TotalAmount;
                result.GrandTotal = item.GrandTotal;
                result.IsActive = true;
                int count = 0;
                foreach (TempSalesDetail itemDetails in item.TempSalesDetails)
                {
                    SalesDetail resultDetail = new SalesDetail()
                    {
                        SalesMasterId = result.Id,
                        SalesInvoice = result.SalesInvoice,
                        ProductId = itemDetails.ProductId,
                        BaleQty = itemDetails.BaleQty,
                        BaleWeight = itemDetails.BaleWeight,
                        TotalQtyInKG = itemDetails.TotalQtyInKG,
                        Rate = itemDetails.Rate,
                        Amount = itemDetails.Amount,
                        Notes = FinalResult.Notes,
                        CreatedBy = CurrentSession.GetCurrentSession().UserName,
                        CreatedDate = new DateTime?(DateTime.Now),
                        IsActive = new bool?(true),
                        WarehouseId = itemDetails.WarehouseId
                    };
                    count += itemDetails.BaleQty;
                    result.SalesDetails.Add(resultDetail);
                }
                message = string.Concat(message, "Tk=", string.Format("{0:#,#.}", decimal.Round(result.GrandTotal), ""), "=");
                if (item.SalesOrder != null)
                {
                    string orderid = "";
                    SalesOrder order = this.salesService.GetSalesOrderById(new int?(Convert.ToInt32(item.SalesOrderId)));
                    orderid = string.Concat(",", order.SalesOrderId);
                    message = string.Concat(message, orderid);
                }
                message = string.Concat(message, ",SO-", result.SalesInvoice);
                message = string.Concat(message, ",T/No:", item.TransportType + ",Mob:" + result.TransportNo);

                result.DriverName =item.DriverName;
                result.RentAmount = item.RentAmount;
                SalesMaster saved = this.salesService.SaveSalesMaster(result);
                if (item.SalesOrder != null)
                {
                    item.SalesOrder.DeliveryDate = new DateTime?(DateTime.Now);
                    SalesOrder order = this.salesService.GetSalesOrderById(new int?(Convert.ToInt32(item.SalesOrderId)));
                    SalesOrder salesOrder = order;
                    deliveryQty = order.DeliveryQty;
                    baleQty =order.BaleQty;
                    if (deliveryQty.HasValue)
                    {
                        nullable = new decimal?(deliveryQty.GetValueOrDefault() + baleQty);
                    }
                    else
                    {
                        nullable = null;
                    }
                    salesOrder.DeliveryQty = nullable;
                    order.BaleQty = order.BaleQty - order.BaleQty;
                    order.DeliveryDate = new DateTime?(DateTime.Now);
                    order.IsActive = false;
                    FinalResultOrder = this.salesService.Update(order, order.Id);
                }
                if (saved.Id > 0)
                {
                    if (item.CustomerID != 0)
                    {
                        Customer customer = (new CustomerService()).GetById(new int?(item.CustomerID));
                        int? ledgerId = customer.LedgerId;
                        rptIndividualLedger_Result due = customerService.GetBalance((ledgerId.HasValue ? ledgerId.GetValueOrDefault() : 0));
                        string balanceText = "";
                        deliveryQty = due.Balance;
                        baleQty = new decimal();
                        if ((deliveryQty.GetValueOrDefault() < baleQty ? !deliveryQty.HasValue : true))
                        {
                            deliveryQty = due.Balance;
                            balanceText = string.Concat("Balance with Dada Rice Tk=", string.Format("{0:#,#.}", decimal.Round((deliveryQty.HasValue ? deliveryQty.GetValueOrDefault() : decimal.Zero)), ""), "=");
                        }
                        else
                        {
                            decimal minusOne = decimal.MinusOne;
                            deliveryQty = due.Balance;
                            balanceText = string.Concat("Balance with Dada Rice Tk=", string.Format("{0:#,#.}", minusOne * decimal.Round((deliveryQty.HasValue ? deliveryQty.GetValueOrDefault() : decimal.Zero)), ""), "=");
                        }
                        SMSEmailService sMSEmailService = new SMSEmailService();
                        string phone = customer.Phone;
                        string[] str = new string[] { "Dear Customer,Del Qty=", count.ToString(), " BAGS ", message, ",Dated:", null, null, null };
                        str[5] = DateTime.Now.ToString("dd-MM-yyyy");
                        str[6] = ".";
                        str[7] = balanceText;
                        if (isSendSMS)
                        {
                            sMSEmailService.SendOneToOneSingleSms(phone, string.Concat(str));
                        }
                    }
                    //update temp
                    item.IsActive = false;
                    item.UpdatedDate = DateTime.Now.ToString();
                    this.salesService.Update(item, id);
                }
                var r = AutoMapper.Mapper.Map<SalesMaster, SalesMasterResponse>(saved);
                actionResult = base.Json(r, 0);
            }
            catch (Exception exception)
            {
                exception.ErrorWritter();
                actionResult = base.Json("Error", 0);
            }
            return actionResult;
        }
        public ActionResult GetAllSales(int CustomerID, string fromDate, string toDate)
        {
            ActionResult actionResult;
            DateTime cFromDate = Convert.ToDateTime(fromDate).Date;
            DateTime cToDate = Convert.ToDateTime(toDate).Date;
            List<SalesMaster> category = null;
            if (CustomerID != 0)
            {
               category= this.salesService.GetAllSalesFilteredByCustomer(CustomerID);
            }
            else
            {
                category = this.salesService.GetAll(cFromDate, cToDate);
            }
            if (category != null)
            {
                actionResult = base.Json(Mapper.Map<List<SalesMaster>, List<SalesMasterResponse>>(category), 0);
            }
            else
            {
                actionResult = base.HttpNotFound();
            }
            return actionResult;
        }
        public ActionResult GetProductWiseSalesSummary()
        {
            List<TopSellResponse> category = salesService.GetTopSell();
            if (category == null)
            {
                return HttpNotFound();
            }
            return Json(category, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllSalesFilteredByCustomer(int id)
        {
            List<SalesMaster> sales = salesService.GetAllSalesFilteredByCustomer(id);
            if (sales == null)
            {
                return HttpNotFound();
            }

            var result = AutoMapper.Mapper.Map<List<SalesMaster>, List<SalesMasterResponse>>(sales);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllSalesFilteredByDate(DateTime startDate, DateTime endDate)
        {
            List<SalesMaster> sales = salesService.GetAllSalesFilteredByDateForReport(startDate, endDate);
            if (sales == null)
            {
                return HttpNotFound();
            }

            var result = AutoMapper.Mapper.Map<List<SalesMaster>, List<SalesMasterResponse>>(sales);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllSalesFilteredByDateAndCustomer(DateTime startDate, DateTime endDate, int customerId)
        {
            List<SalesMaster> sales =
                salesService.GetAllSalesFilteredByDateAndCustomerForReport(startDate, endDate, customerId);
            if (sales == null)
            {
                return HttpNotFound();
            }

            var result = AutoMapper.Mapper.Map<List<SalesMaster>, List<SalesMasterResponse>>(sales);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetInvoiceNumber()
        {
            var invoiceNumber = GetInvoiceNo();
            return Json(invoiceNumber, JsonRequestBehavior.AllowGet);
        }

        public string GetInvoiceNo()
        {
            string invoiceNumber = "DR" + DateTime.Now.Year +
                new GlobalClass().GetInvoiceNo("Id", "SalesMaster");
            return invoiceNumber;
        }

        public ActionResult GetInvoiceNumberSalesOrder()
        {
            string invoiceNumber = "DO" + DateTime.Now.Year +"-"+
                new GlobalClass().GetSalesOrder();
            return Json(invoiceNumber, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllInventory()
        {
            List<Inventory> inventories = inventoryService.GetAll();
            if (inventories == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<Inventory>, List<InventoryResponse>>(inventories);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllInventoryforSales()
        {
            List<Inventory> inventories = inventoryService.GetAllForSale();
            if (inventories == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<Inventory>, List<InventoryResponse>>(inventories);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllSaleOrders()
        {
            List<SalesOrder> salesOrders = salesService.GetAllSalesOrders();
            if (salesOrders == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<SalesOrder>, List<SalesOrderResponse>>(salesOrders);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetSaleOrders(string salesOrderId)
        {
            ActionResult actionResult;
            List<SalesOrder> salesOrders = this.salesService.GetAllSalesOrdersByOrderId(salesOrderId);
            if (salesOrders != null)
            {
                actionResult = base.Json(Mapper.Map<List<SalesOrder>, List<SalesOrderResponse>>(salesOrders), 0);
            }
            else
            {
                actionResult = base.HttpNotFound();
            }
            return actionResult;
        }
        public ActionResult GetAllSaleOrdersFilterdByCustomer(int? CustomerID)
        {
            List<SalesOrder> salesOrders = salesService.GetAllSalesOrdersByCustomerId(CustomerID);
            if (salesOrders == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<SalesOrder>, List<SalesOrderResponse>>(salesOrders);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSaleOrdersSummary(int? CustomerID, string fromDate, string toDate)
        {
            ActionResult actionResult;
            List<SalesOrder> salesOrders = new List<SalesOrder>();
            List<SalesOrderResponse> responses = new List<SalesOrderResponse>();
            DateTime cFromDate = Convert.ToDateTime(fromDate).Date;
            DateTime cToDate = Convert.ToDateTime(toDate).Date;
            int? customerID = CustomerID;
            salesOrders = ((customerID.GetValueOrDefault() == 0 ? !customerID.HasValue : true) ? this.salesService.GetAllSalesOrdersByCustomerId(CustomerID) : this.salesService.GetAllSalesOrders(cFromDate, cToDate));
            if (salesOrders != null)
            {
                foreach (SalesOrder salesOrder in salesOrders)
                {
                    IEnumerable<SalesOrderResponse> isexists =
                        from a in responses
                        where a.SalesOrderId == salesOrder.SalesOrderId
                        select a;
                    if ((isexists == null ? true : isexists.Count<SalesOrderResponse>() <= 0))
                    {
                        SalesOrderResponse response = new SalesOrderResponse()
                        {
                            Id = salesOrder.Id,
                            SalesOrderId = salesOrder.SalesOrderId,
                            OrderDate = salesOrder.OrderDate,
                            PostDate = salesOrder.OrderDate.ToString("dd-MM-yyyy")
                        };
                        response.Customer.Name = salesOrder.Customer.Name;
                        response.Amount = salesOrder.Amount;
                        response.Notes = string.Concat(new object[] { salesOrder.Product.ProductName, "->", salesOrder.BaleQty, "*", salesOrder.BaleWeight, "*", salesOrder.Rate });
                        responses.Add(response);
                    }
                    else
                    {
                        SalesOrderResponse salesOrderResponse = isexists.FirstOrDefault<SalesOrderResponse>();
                        salesOrderResponse.Notes = string.Concat(new object[] { salesOrderResponse.Notes, " ,", salesOrder.Product.ProductName, "->", salesOrder.BaleQty, "*", salesOrder.BaleWeight, "*", salesOrder.Rate });
                        SalesOrderResponse amount = salesOrderResponse;
                        amount.Amount = amount.Amount + salesOrder.Amount;
                    }
                }
                actionResult = base.Json(responses, 0);
            }
            else
            {
                actionResult = base.HttpNotFound();
            }
            return actionResult;
        }

        public ActionResult GetSalesDetailsByVoucher(string voucherNo)
        {
            if (voucherNo.Contains("DR")||voucherNo.Contains("dr")||voucherNo.Contains("SO"))
            {

                List<SalesDetail> category = this.salesService.GetAllSalesDetails(voucherNo);
                if (category == null)
                {
                    category = new List<SalesDetail>();
                }
                var result = AutoMapper.Mapper.Map<List<SalesDetail>, List<SalesDetailResponse>>(category);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result=Mapper.Map<List<LedgerPostingResponse>>(service.GetByVoucherNo(voucherNo));
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetSaleOrdersFilter(int? CustomerID, string fromDate, string toDate)
        {
            List<SalesOrder> salesOrders = new List<SalesOrder>();
            List<SalesOrderResponse> responses = new List<SalesOrderResponse>();
            DateTime cFromDate = Convert.ToDateTime(fromDate).Date;
            DateTime cToDate = Convert.ToDateTime(toDate).Date;
            int? customerID = CustomerID;
            salesOrders = ((customerID.GetValueOrDefault() == 0 ? !customerID.HasValue : true) ? this.salesService.GetAllSalesOrdersByCustomerId(CustomerID) : this.salesService.GetAllSalesOrders(cFromDate, cToDate));
            var chouthaNo = salesService.GetChoutha(CustomerID ?? 0);
            var salesOrder = new List<SalesOrderResponse>();
            if (salesOrder != null)
            {
                salesOrder = Mapper.Map<List<SalesOrder>, List<SalesOrderResponse>>(salesOrders);
            }
            int i = 0;
            if (string.IsNullOrEmpty(chouthaNo))
            {
                i =0;
            }
            else
            {
                if (!Int32.TryParse(chouthaNo, out i))
                {
                    i = 0;
                }
            }
            var customer = customerService.GetById(customerID);
            var balanceObj = customerService.GetBalance(customer.LedgerId??0);
            var balance = 0.0m;
            if (balanceObj != null)
            {
                balance = balanceObj.Balance ?? 0.0m;

            }
            var result = new { salesOrders = salesOrder, chouthaNo = i+1,Balance=balance,Limit=customer.Limit};
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveSalesOrder(List<SalesOrder> salesOrders,bool isSendSMS)
        {
            SalesOrder result = new SalesOrder();
            SalesOrder FinalResult = new SalesOrder();
            decimal totalAmount = new decimal();
            int customerId = 0;
            int count = 1;
            foreach (SalesOrder item in salesOrders)
            {
                result.SalesOrderId = item.SalesOrderId;
                result.CustomerID = item.CustomerID;
                result.Notes = item.Notes;
                result.OrderDate = DateTime.Now.Date;
                result.OrderRecieveBy = CurrentSession.GetCurrentSession().UserName;
                result.ProductId = item.ProductId;
                result.BaleQty = item.BaleQty;
                result.BaleWeight = item.BaleWeight;
                result.TotalQtyInKG = item.TotalQtyInKG;
                result.Rate = item.Rate;
                result.Amount = item.Amount;
                result.DeliveryDate = item.DeliveryDate;
                result.DeliveryQty = new decimal?(new decimal());
                result.CreatedBy = CurrentSession.GetCurrentSession().UserName;
                result.CreatedDate = new DateTime?(DateTime.Now);
                result.IsActive = true;
                FinalResult = this.salesService.SaveSalesOrder(result);
                totalAmount += result.Amount;
                customerId = item.CustomerID;
                count++;
            }
            if (FinalResult.Id > 0)
            {
                if (isSendSMS)
                {
                    Customer customer = (new CustomerService()).GetById(new int?(customerId));
                    SMSEmailService sMSEmailService = new SMSEmailService();
                    string phone = customer.Phone;
                    string[] name = new string[] { "Dear ", customer.Name, ",DO Has Been Created.DO No-", FinalResult.SalesOrderId, ", Dated- ", null, null, null, null };
                    name[5] = DateTime.Now.ToString("dd-MM-yyyy");
                    name[6] = ". Total DO Amount =";
                    name[7] = string.Format("{0:#,##0}", decimal.Round(totalAmount), "");
                    name[8] = "/= Dada Rice.";
                    sMSEmailService.SendOneToOneSingleSms(phone, string.Concat(name));
                }
            }
            return Json(FinalResult,JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Export()
        //{
        //    List<SalesMaster> allCustomer = new List<SalesMaster>();
        //    allCustomer = context.SalesMasters.ToList();
        //    var data = AutoMapper.Mapper.Map<List<SalesMaster>, List<SalesMasterResponse>>(allCustomer, new List<SalesMasterResponse>());
        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/Report/RPT"), "rptDeliveryOrder.rpt"));
        //    rd.SetDataSource(data);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();


        //    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    stream.Seek(0, SeekOrigin.Begin);
        //    return File(stream, "application/pdf", "SalesInvoice.pdf");
        //}

        [HttpPost]
        public ActionResult SaveSales(List<TempSalesMaster> salesMasters, decimal Discount, List<TempSalesDetail> salesDetail, List<SalesOrder> salesOrders, List<string> lstDeliveryQunatities, bool isSendSMS, string DriverName, decimal RentAmount = 0.0m)
        {
            var salesinvoiceid = GetInvoiceNo();
            ActionResult actionResult;
            int customerId = 0;
            TempSalesMaster result = new TempSalesMaster();
            result.SalesInvoice = salesinvoiceid;
            foreach (var item in salesMasters)
            {
                item.SalesInvoice = salesinvoiceid;
            }
            TempSalesMaster FinalResult = new TempSalesMaster();
            FinalResult.SalesInvoice = salesinvoiceid;
            TempSalesDetail FinalResultDetail = new TempSalesDetail();
            SalesOrder resultOrder = new SalesOrder();
            SalesOrder FinalResultOrder = new SalesOrder();

            List<int> lstSalesMasterId = new List<int>();
            try
            {
                foreach (TempSalesMaster item in salesMasters)
                {
                    customerId = item.CustomerID;
                    result.SalesInvoice = item.SalesInvoice;
                    result.SalesOrderId = item.SalesOrderId;
                    result.SalesDate = item.SalesDate;
                    if (!result.SalesDate.HasValue)
                    {
                        result.SalesDate = new DateTime?(DateTime.Now);
                    }
                    result.SalesBy = CurrentSession.GetCurrentSession().UserName;
                    result.CustomerID = item.CustomerID;
                    result.AdditionalCost = decimal.Zero;
                    result.Discount = Discount;
                    result.Notes = item.Notes;
                    result.Coutha = item.Coutha;
                    result.TransportNo = item.TransportNo;
                    result.TransportType = item.TransportType;
                    result.CreatedBy = CurrentSession.GetCurrentSession().UserName;
                    result.CreatedDate = new DateTime?(DateTime.Now);
                    result.IsActive = true;
                }
                result.TotalAmount = (
                    from a in salesMasters
                    select a.TotalAmount).Sum();
                result.GrandTotal = ((
                    from a in salesMasters
                    select a.TotalAmount).Sum() + result.AdditionalCost) - result.Discount;
                int count = 0;
                foreach (TempSalesDetail item in salesDetail)
                {
                    TempSalesDetail resultDetail = new TempSalesDetail()
                    {
                        SalesMasterId = result.Id,
                        SalesInvoice = result.SalesInvoice,
                        ProductId = item.ProductId,
                        BaleQty = item.BaleQty,
                        BaleWeight = item.BaleWeight,
                        TotalQtyInKG = item.TotalQtyInKG,
                        Rate = item.Rate,
                        Amount = item.Amount,
                        Notes = FinalResult.Notes,
                        CreatedBy = CurrentSession.GetCurrentSession().UserName,
                        CreatedDate = new DateTime?(DateTime.Now),
                        IsActive = new bool?(true),
                        WarehouseId = item.WarehouseId
                    };
                    result.TempSalesDetails.Add(resultDetail);
                    count += item.BaleQty;
                }
                result.DriverName = DriverName;
                result.RentAmount = RentAmount;
                TempSalesMaster saved = this.salesService.SaveTempSalesMaster(result);
                actionResult = base.Json(FinalResult, 0);
            }
            catch (Exception exception)
            {
                exception.ErrorWritter();
                actionResult = base.Json("Error", 0);
            }
            return actionResult;
        }
        //[HttpPost]
        //public ActionResult SaveSales(List<SalesMaster> salesMasters, decimal Discount, List<SalesDetail> salesDetail, List<SalesOrder> salesOrders, List<string> lstDeliveryQunatities,bool isSendSMS,string DriverName,decimal RentAmount=0.0m)
        //{
        //    var salesinvoiceid = "DR" + DateTime.Now.Year +
        //        new GlobalClass().GetMaxId("Id", "SalesMaster");

        //    decimal? deliveryQty;
        //    decimal baleQty;
        //    ActionResult actionResult;
        //    decimal? nullable;
        //    int customerId = 0;
        //    string message = "";
        //    SalesMaster result = new SalesMaster();
        //    result.SalesInvoice = salesinvoiceid;
        //    foreach (var item in salesMasters)
        //    {
        //        item.SalesInvoice = salesinvoiceid;
        //    }
        //    SalesMaster FinalResult = new SalesMaster();           
        //    FinalResult.SalesInvoice = salesinvoiceid;
        //    SalesDetail FinalResultDetail = new SalesDetail();
        //    SalesOrder resultOrder = new SalesOrder();
        //    SalesOrder FinalResultOrder = new SalesOrder();

        //    List<int> lstSalesMasterId = new List<int>();
        //    try
        //    {
        //        foreach (SalesMaster item in salesMasters)
        //        {
        //            customerId = item.CustomerID;
        //            result.SalesInvoice = item.SalesInvoice;
        //            result.SalesOrderId = item.SalesOrderId;
        //            result.SalesDate = item.SalesDate;
        //            if (!result.SalesDate.HasValue)
        //            {
        //                result.SalesDate = new DateTime?(DateTime.Now);
        //            }
        //            result.SalesBy = CurrentSession.GetCurrentSession().UserName;
        //            result.CustomerID = item.CustomerID;
        //            result.AdditionalCost = decimal.Zero;
        //            result.Discount = Discount;
        //            result.Notes = item.Notes;
        //            result.Coutha = item.Coutha;
        //            result.TransportNo = item.TransportNo;
        //            result.TransportType = item.TransportType;
        //            result.CreatedBy = CurrentSession.GetCurrentSession().UserName;
        //            result.CreatedDate = new DateTime?(DateTime.Now);
        //            result.IsActive = true;
        //        }
        //        result.TotalAmount = (
        //            from a in salesMasters
        //            select a.TotalAmount).Sum();
        //        result.GrandTotal = ((
        //            from a in salesMasters
        //            select a.TotalAmount).Sum() + result.AdditionalCost) - result.Discount;
        //        int count = 0;
        //        foreach (SalesDetail item in salesDetail)
        //        {
        //            SalesDetail resultDetail = new SalesDetail()
        //            {
        //                SalesMasterId = result.Id,
        //                SalesInvoice = result.SalesInvoice,
        //                ProductId = item.ProductId,
        //                BaleQty = item.BaleQty,
        //                BaleWeight = item.BaleWeight,
        //                TotalQtyInKG = item.TotalQtyInKG,
        //                Rate = item.Rate,
        //                Amount = item.Amount,
        //                Notes = FinalResult.Notes,
        //                CreatedBy = CurrentSession.GetCurrentSession().UserName,
        //                CreatedDate = new DateTime?(DateTime.Now),
        //                IsActive = new bool?(true),
        //                WarehouseId = item.WarehouseId
        //            };
        //            result.SalesDetails.Add(resultDetail);
        //            count += item.BaleQty;
        //        }
        //        message = string.Concat(message, "Tk=", string.Format("{0:#,#.}", decimal.Round(result.GrandTotal), ""), "=");
        //        if (salesOrders != null)
        //        {
        //            string orderid = "";
        //            for (int i = 0; i < salesOrders.Count; i++)
        //            {
        //                SalesOrder order = this.salesService.GetSalesOrderById(new int?(Convert.ToInt32(salesOrders[i].SalesOrderId)));
        //                orderid = string.Concat(",", order.SalesOrderId);
        //            }
        //            message = string.Concat(message, orderid);
        //        }
        //        message = string.Concat(message, ",SO-", result.SalesInvoice);
        //        message = string.Concat(message, ",T/No:", salesMasters.FirstOrDefault<SalesMaster>().TransportType+ ",Mob:"+result.TransportNo);

        //        result.DriverName = DriverName;
        //        result.RentAmount = RentAmount;
        //        SalesMaster saved = this.salesService.SaveSalesMaster(result);
        //        if (salesOrders != null)
        //        {
        //            for (int i = 0; i < salesOrders.Count; i++)
        //            {
        //                salesOrders[i].DeliveryDate = new DateTime?(DateTime.Now);
        //                SalesOrder order = this.salesService.GetSalesOrderById(new int?(Convert.ToInt32(salesOrders[i].SalesOrderId)));
        //                SalesOrder salesOrder = order;
        //                deliveryQty = order.DeliveryQty;
        //                baleQty = salesOrders[i].BaleQty;
        //                if (deliveryQty.HasValue)
        //                {
        //                    nullable = new decimal?(deliveryQty.GetValueOrDefault() + baleQty);
        //                }
        //                else
        //                {
        //                    nullable = null;
        //                }
        //                salesOrder.DeliveryQty = nullable;
        //                order.BaleQty = order.BaleQty - salesOrders[i].BaleQty;
        //                order.DeliveryDate = new DateTime?(DateTime.Now);
        //                order.IsActive = salesOrders[i].IsActive;
        //                FinalResultOrder = this.salesService.Update(order, order.Id);
        //            }
        //        }
        //        if (saved.Id > 0)
        //        {
        //            if (customerId != 0)
        //            {
        //                Customer customer = (new CustomerService()).GetById(new int?(customerId));
        //                int? ledgerId = customer.LedgerId;
        //                rptIndividualLedger_Result due = customerService.GetBalance((ledgerId.HasValue ? ledgerId.GetValueOrDefault() : 0));
        //                string balanceText = "";
        //                deliveryQty = due.Balance;
        //                baleQty = new decimal();
        //                if ((deliveryQty.GetValueOrDefault() < baleQty ? !deliveryQty.HasValue : true))
        //                {
        //                    deliveryQty = due.Balance;
        //                    balanceText = string.Concat("Balance with Dada Rice Tk=", string.Format("{0:#,#.}", decimal.Round((deliveryQty.HasValue ? deliveryQty.GetValueOrDefault() : decimal.Zero)), ""), "=");
        //                }
        //                else
        //                {
        //                    decimal minusOne = decimal.MinusOne;
        //                    deliveryQty = due.Balance;
        //                    balanceText = string.Concat("Balance with Dada Rice Tk=", string.Format("{0:#,#.}", minusOne * decimal.Round((deliveryQty.HasValue ? deliveryQty.GetValueOrDefault() : decimal.Zero)), ""), "=");
        //                }
        //                SMSEmailService sMSEmailService = new SMSEmailService();
        //                string phone = customer.Phone;
        //                string[] str = new string[] { "Dear Customer,Del Qty=", count.ToString(), " BAGS ", message, ",Dated:", null, null, null };
        //                str[5] = DateTime.Now.ToString("dd-MM-yyyy");
        //                str[6] = ".";
        //                str[7] = balanceText;
        //                if (isSendSMS)
        //                {
        //                    sMSEmailService.SendOneToOneSingleSms(phone, string.Concat(str));
        //                }
        //            }
        //        }
        //        actionResult = base.Json(FinalResult, 0);
        //    }
        //    catch (Exception exception)
        //    {
        //        exception.ErrorWritter();
        //        actionResult = base.Json("Error", 0);
        //    }
        //    return actionResult;
        //}

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMaster sales = salesService.GetById(id);
            if (sales == null)
            {
                return HttpNotFound();
            }
            //if (sales.SalesDate.Value.AddDays(3) > DateTime.Now)
            //{
            //    return View("Index");
            //}
            sales.SalesDate = DateTime.Now;
            var result = salesService.DeleteSales(sales);
            return View("Index");
        }
        [HttpGet]
        public ActionResult TempSalesDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempSalesMaster sales = salesService.GetByTempId(id);
            if (sales == null)
            {
                return HttpNotFound();
            }
            //if (sales.SalesDate.Value.AddDays(3) > DateTime.Now)
            //{
            //    return View("Index");
            //}
            sales.SalesDate = DateTime.Now;
            var result = salesService.DeleteTempSales(sales);
            return View("Autorized");
        }
        [HttpPost]
        public ActionResult DeleteSalesOrder(int Id)
        {
            ActionResult httpStatusCodeResult;
            if (Id != null)
            {
                SalesOrder salesList = this.salesService.GetSalesOrderById(Id);
                bool isAlreadyUsed = false;
                if (this.salesService.GetAll(new int?(Id)).FirstOrDefault<SalesMaster>() != null)
                {
                    isAlreadyUsed = true;
                    httpStatusCodeResult = base.HttpNotFound();
                    return httpStatusCodeResult;
                }
                if (!isAlreadyUsed)
                {

                    SalesOrder salesOrder = this.salesService.GetSalesOrderById(new int?(Id));
                    if (salesOrder != null)
                    {
                        salesOrder.OrderDate = DateTime.Now;
                        salesOrder.IsActive = false;
                        this.salesService.Update(salesOrder, salesOrder.Id);
                    }
                    else
                    {
                        httpStatusCodeResult = base.HttpNotFound();
                        return httpStatusCodeResult;
                    }
                    Customer customer = (new CustomerService()).GetById(salesOrder.CustomerID);
                    SMSEmailService sMSEmailService = new SMSEmailService();
                    string[] name = new string[] { "Dear ", customer.Name, ",Your DO was wrong posted. Your DO Has Been Deleted. DO No-", salesOrder.SalesOrderId, ", Dated- ", null, null };
                    name[5] = DateTime.Now.ToString("dd-MM-yyyy");
                    name[6] = ". /= Dada Rice.";
                    sMSEmailService.SendOneToOneSingleSms(customer.Phone, string.Concat(name));
                }
                httpStatusCodeResult = base.View("Index");
            }
            else
            {
                httpStatusCodeResult = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return httpStatusCodeResult;

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = salesService.GetSalesOrderById(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<SalesOrder, SalesOrderResponse>(salesOrder);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}