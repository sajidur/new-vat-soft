using Microsoft.Build.Framework;
using Microsoft.Owin.Logging;
using REX_MVC.Models;
using REX_MVC.Util;
using REX_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace REX_MVC.BAL
{
    public class SalesService
    {  // SQLDAL objSqlData = new SQLDAL();
        Result oResult = new Result();
        DBService<SalesDetail> salesDetailsService = new DBService<SalesDetail>();
        Entities entity = new Entities();
        InventoryService inventoryService = new InventoryService();
        DBService<TopSellResponse> topSellResponse = new DBService<TopSellResponse>();
        DBService<SalesOrder> serviceSalesOrder = new DBService<SalesOrder>();
        DBService<SalesMaster> serviceSalesMaster = new DBService<SalesMaster>();
        DBService<TempSalesMaster> serviceTempSalesMaster = new DBService<TempSalesMaster>();

        DBService<Inventory> inventory = new DBService<Inventory>();
        LedgerPostingService ledgerService = new LedgerPostingService();
        CustomerService customerService = new CustomerService();
        PartyBalanceService partyBalanceService = new PartyBalanceService();
        int yearId = CurrentSession.GetCurrentSession().FinancialYear;
        public List<SalesMaster> GetAll()
        {
            return serviceSalesMaster.GetAll(a => a.IsActive == true).ToList();
        }

        public List<SalesMaster> GetAll(int? orderId)
        {
            return serviceSalesMaster.GetAll(a => a.IsActive == true && a.SalesOrderId==orderId && a.YearId== yearId).ToList();
        }
        public string GetChoutha(int  customerId)
        {
            var salesmaster= serviceSalesMaster.GetAll(a => a.CustomerID == customerId && a.YearId== yearId).OrderByDescending(a=>a.Id).FirstOrDefault();
            if (salesmaster==null)
            {
                return "";
            }
            else
            {
              return  salesmaster.Coutha;
            }
        }
        public List<SalesMaster> GetAll(DateTime fromDate, DateTime toDate)
        {
            List<SalesMaster> list = this.serviceSalesMaster.GetAll((SalesMaster a) => (a.SalesDate >= (DateTime?)fromDate) && (a.SalesDate <= (DateTime?)toDate) && a.IsActive && a.YearId== yearId).ToList<SalesMaster>();
            return list;
        }


        public List<SalesMaster> GetAllSalesFilteredByCustomer(int customerId)
        {
            return serviceSalesMaster.GetAll(a => a.CustomerID == customerId && a.IsActive == true && a.YearId== yearId).ToList();
        }

        public List<SalesMaster> GetAllSalesFilteredByDateForReport(DateTime startDate, DateTime endDate)
        {
            return serviceSalesMaster.GetAll(a => a.IsActive == true && (a.CreatedDate >= startDate && a.CreatedDate < endDate) && a.YearId== yearId).ToList();
        }

        public List<SalesMaster> GetAllSalesFilteredByDateAndCustomerForReport(DateTime startDate, DateTime endDate, int customerId)
        {
            return serviceSalesMaster.GetAll(a => a.IsActive == true && (a.CreatedDate >= startDate && a.CreatedDate < endDate) && a.CustomerID == customerId && a.YearId== yearId).ToList();
        }

        public List<TopSellResponse> GetTopSell()
        {
            //var result = (from i in entity.SalesDetails
            //              group i by i.ProductId into g
            //              select new TopSellResponse()
            //              {
            //                  ItemId = g.Key,
            //                  ProductName = g.Where(a => a.ProductId == g.Key).FirstOrDefault().Product.ProductName,
            //              }).ToList().OrderByDescending(a => a.SalesQty);
            DataTable dt = null;
            dt = oResult.Data as DataTable;
            return topSellResponse.ExecuteProcedure("ProductWiseBestSales");

        }
        public SalesMaster GetById(int? id = 0)
        {
            return serviceSalesMaster.GetById(id);
        }
        public TempSalesMaster GetByTempId(int? id = 0)
        {
            return serviceTempSalesMaster.GetById(id);
        }
        public int Delete(int id)
        {
            return serviceSalesMaster.Delete(id);
        }

        public  decimal GetTodaysSales(DateTime  clientDate)
        {
            try
            {
                return serviceSalesMaster.GetAll(a => a.IsActive == true && DbFunctions.TruncateTime(a.SalesDate).Value == clientDate.Date).Sum(a => a.GrandTotal);

            }
            catch (Exception)
            {

                return 0;            }
        }
        public List<TempSalesMaster> GetUnAurhorizedSales()
        {
            try
            {
                return serviceTempSalesMaster.GetAll(a => a.IsActive == true).ToList();

            }
            catch (Exception)
            {

                return new List<TempSalesMaster>();
            }
        }
        public int DeleteTempSales(TempSalesMaster temp)
        {
            try
            {
                foreach (TempSalesDetail detaisl in temp.TempSalesDetails.ToList())
                {
                    (new DBService<TempSalesDetail>()).Delete(detaisl.Id);
                }
                  return
                    (new DBService<TempSalesMaster>()).Delete(temp.Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Sales order service
        public SalesOrder SaveSalesOrder(SalesOrder salesOrder)
        {
            return serviceSalesOrder.Save(salesOrder);
        }

        public int DeleteSales(SalesMaster salesMaster)
        {
            int num;
            try
            {
                IQueryable<SalesDetail> salesdetails = this.salesDetailsService.GetAll((SalesDetail a) => a.SalesMasterId == salesMaster.Id);
                foreach (SalesDetail item in salesdetails)
                {
                    (new DBService<SalesDetail>()).Delete(item.Id);
                }
                this.serviceSalesMaster.Delete(salesMaster.Id);
                foreach (SalesDetail salesDetail in salesMaster.SalesDetails)
                {
                    List<Inventory> existingItem = this.inventory.GetAll((Inventory a) => a.ProductId == salesDetail.ProductId && a.IsActive && a.QtyInBale == (int?)salesDetail.Qty && a.WarehouseId == salesDetail.WarehouseId).ToList<Inventory>();
                    if (existingItem.Count > 0)
                    {
                        foreach (Inventory inv in existingItem)
                        {
                            inv.UpdatedDate = new DateTime?(DateTime.Now);
                            inv.UpdatedBy = "";
                            Inventory nullable = inv;
                            decimal? salesQty = inv.SalesQty;
                            nullable.SalesQty = new decimal?((salesQty.HasValue ? salesQty.GetValueOrDefault() : 0 - salesDetail.Qty));
                            inv.BalanceQty = inv.BalanceQty + salesDetail.Qty;
                            this.inventory.Update(inv, inv.Id);
                        }
                    }
                }
                Customer customer = this.customerService.GetById(new int?(salesMaster.CustomerID));
                foreach (LedgerPosting item in this.ledgerService.GetAll(salesMaster.SalesInvoice, true))
                {
                    item.IsActive = false;
                    this.ledgerService.Update(item, item.Id);
                }
                PartyBalanceService partyBalanceService = this.partyBalanceService;
                int? ledgerId = customer.LedgerId;
                PartyBalance paymentObj = partyBalanceService.GetByVoucher(customer.LedgerId??0, salesMaster.SalesInvoice);
                this.partyBalanceService.Delete(paymentObj.PartyBalanceId);
                num = 1;
            }
            catch (Exception exception)
            {
                num = 0;
            }
            return num;

        }

        public List<SalesDetail> GetAllSalesDetails(string voucherNo)
        {
            List<SalesDetail> salesDetails1;
            List<SalesDetail> salesDetails = new List<SalesDetail>();
            SalesMaster sales = this.serviceSalesMaster.GetAll((SalesMaster a) => a.SalesInvoice == voucherNo && a.IsActive).FirstOrDefault<SalesMaster>();
            salesDetails1 = (sales == null ? new List<SalesDetail>() : sales.SalesDetails.ToList<SalesDetail>());
            return salesDetails1;
        }
        public TempSalesMaster SaveTempSalesMaster(TempSalesMaster salesMaster)
        {
            salesMaster.YearId = yearId;
            TempSalesMaster result = this.serviceTempSalesMaster.Save(salesMaster);
            return result;

        }

        public SalesMaster SaveSalesMaster(SalesMaster salesMaster)
        {
            salesMaster.YearId = yearId;
            SalesMaster result = this.serviceSalesMaster.Save(salesMaster);
            if ((result != null ? true : result.Id > 0))
            {
                foreach (SalesDetail salesDetail in salesMaster.SalesDetails)
                {
                    List<Inventory> existingItem = this.inventory.GetAll((Inventory a) => a.ProductId == salesDetail.ProductId && a.IsActive && a.WarehouseId == salesDetail.WarehouseId).ToList<Inventory>();
                    if (existingItem.Count > 0)
                    {
                        foreach (Inventory inv in existingItem)
                        {
                            inv.UpdatedDate = new DateTime?(DateTime.Now);
                            inv.UpdatedBy = "";
                            Inventory nullable = inv;
                            decimal? salesQty = inv.SalesQty;
                            nullable.SalesQty = new decimal?((salesQty.HasValue ? salesQty.GetValueOrDefault() : salesDetail.Qty));
                            inv.BalanceQty = inv.BalanceQty - salesDetail.Qty;
                            this.inventory.Update(inv, inv.Id);
                        }
                    }
                }
                Customer customer = this.customerService.GetById(new int?(salesMaster.CustomerID));
                LedgerPosting ledgerObj = new LedgerPosting()
                {
                    VoucherTypeId = new int?(19),
                    VoucherNo = result.SalesInvoice,
                    PostingDate = salesMaster.SalesDate,
                    LedgerId = new int?(10),
                    InvoiceNo = result.SalesInvoice,
                    Credit = new decimal?(result.GrandTotal)
                };
                decimal num = new decimal();
                ledgerObj.Debit = new decimal?(num);
                this.ledgerService.Save(ledgerObj);
                LedgerPosting detailsLedger = new LedgerPosting()
                {
                    VoucherTypeId = new int?(19),
                    VoucherNo = result.SalesInvoice,
                    PostingDate = salesMaster.SalesDate,
                    LedgerId = customer.LedgerId,
                    InvoiceNo = result.SalesInvoice
                };
                num = new decimal();
                detailsLedger.Credit = new decimal?(num);
                detailsLedger.Debit = new decimal?(salesMaster.GrandTotal);
                this.ledgerService.Save(detailsLedger);
                PartyBalance balance = new PartyBalance()
                {
                    AgainstInvoiceNo = result.SalesInvoice,
                    AgainstVoucherNo = result.SalesInvoice,
                    AgainstVoucherTypeId = new int?(19),
                    VoucherNo = result.SalesInvoice,
                    PostingDate = salesMaster.SalesDate
                };
                PartyBalance partyBalance = balance;
                int? ledgerId = customer.LedgerId;
                partyBalance.LedgerId = (ledgerId.HasValue ? ledgerId.GetValueOrDefault() : 0);
                balance.InvoiceNo = result.SalesInvoice;
                balance.Debit = new decimal?(salesMaster.GrandTotal);
                num = new decimal();
                balance.Credit = new decimal?(num);
                balance.VoucherTypeId = 19;
                balance.extra1 = string.Concat("Sales Invoice: ", salesMaster.SalesInvoice, " Challan:", salesMaster.Coutha);
                balance.extra2 = result.Id.ToString();
                this.partyBalanceService.Save(balance);
            }
            return result;

        }

        public SalesOrder GetSalesOrderById(int? id = 0)
        {
            return this.serviceSalesOrder.GetById(id);
        }
        public SalesDetail SaveSalesDetail(SalesDetail salesDetail)
        {
            return salesDetailsService.Save(salesDetail);
        }

        public List<SalesOrder> GetAllSalesOrders()
        {
            return serviceSalesOrder.GetAll(a=>a.IsActive==true).ToList();
        }

        public List<SalesOrder> GetAllSalesOrdersByCustomerId(int? CustomerId)
        {
            return serviceSalesOrder.GetAll(c => c.CustomerID == CustomerId && c.IsActive==true).ToList();
        }
        public List<SalesOrder> GetAllSalesOrders(DateTime fromDate, DateTime toDate)
        {
            List<SalesOrder> list = this.serviceSalesOrder.GetAll((SalesOrder a) => (a.OrderDate >= fromDate) && (a.OrderDate <= toDate) && a.IsActive).ToList<SalesOrder>();
            return list;
        }

        public List<SalesOrder> GetAllSalesOrdersByOrderId(string salesOrderId)
        {
            List<SalesOrder> list = this.serviceSalesOrder.GetAll((SalesOrder c) => c.SalesOrderId == salesOrderId && c.IsActive).ToList<SalesOrder>();
            return list;
        }

        public SalesOrder Update(SalesOrder t, int id)
        {
            return serviceSalesOrder.Update(t, id);

        }
        public TempSalesMaster Update(TempSalesMaster t, int id)
        {
            return serviceTempSalesMaster.Update(t, id);
        }
    }
}