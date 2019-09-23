﻿using RiceMill_MVC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RiceMill_MVC.BAL;
using RiceMill_MVC.Models;
using RiceMill_MVC.ViewModel;
using System.Net;
using RiceMill_MVC.Util;

namespace RiceMill_MVC.Controllers
{
    public class ProductionProcessingController : Controller
    {
        // GET: ProductionProcessing
        StockOutService service = new StockOutService();
        private InventoryService inventoryService = new InventoryService();
        private ProductService productService = new ProductService();
        StockOutService stockOutService = new StockOutService();
        StockInService serviceStockIn = new StockInService();
        WareHouseService serviceWareHouseService = new WareHouseService();

        public ActionResult Index()
        {
            List<WareHouse> wareHouses = serviceWareHouseService.GetAll();
            return View(wareHouses);
        }

        public ActionResult StockOut()
        {
            return View(new StockOut());
        }
        public ActionResult StockOutReport()
        {
            return View();
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

        public ActionResult GetInvoiceNumber()
        {
            string invoiceNumber =
                new GlobalClass().GetMaxId("Id","StockOut");
            return Json(invoiceNumber, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInvoiceNumberForStockIn()
        {
            string invoiceNumber =
                new GlobalClass().GetMaxId("Id", "StockIn");
            return Json(invoiceNumber, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save(string InvoiceNo, string Notes, List<StockOut> stockOuts)
        {
            StockOut result = new StockOut();
            StockOut FinalResult = new StockOut();
            //if (ModelState.IsValid)
            {                
                foreach (var item in stockOuts)
                {
                    result.InvoiceNo = InvoiceNo;
                    result.Notes = Notes;
                    result.IsActive = true;
                    result.CreatedDate = DateTime.Now;
                    result.ProductionDate = DateTime.Now;
                    result.CreatedBy = CurrentSession.GetCurrentSession().UserName;

                    result.ProductId = item.ProductId;
                    result.BaleWeight = item.BaleWeight;
                    result.WarehouseId = item.WarehouseId;
                    result.SupplierId = item.SupplierId;
                    result.BaleQty = item.BaleQty;
                    result.WeightInMon = item.WeightInMon;

                    FinalResult = service.Save(result);
                }
            }

            return Json(FinalResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SaveStockIn(string InvoiceNo, string Notes, List<StockIn> stockIns)
        {
            StockIn result = new StockIn();
            StockIn FinalResult = new StockIn();
            //if (ModelState.IsValid)
            {
                foreach (var item in stockIns)
                {
                    result.InvoiceNo = InvoiceNo;
                    result.Notes = Notes;
                    result.IsActive = true;
                    result.CreatedDate = DateTime.Now;
                    result.ProductionDate = DateTime.Now;
                    result.CreatedBy = CurrentSession.GetCurrentSession().UserName;
                    result.SupplierId = 0;
                    result.ProductId = item.ProductId;
                    result.BaleWeight = item.BaleWeight;
                    result.WarehouseId = item.WarehouseId;
                    result.SupplierId = item.SupplierId;
                    result.BaleQty = item.BaleQty;
                    result.WeightInMon = item.WeightInMon;
                    FinalResult = serviceStockIn.Save(result,Notes);
                }
            }

            return Json(FinalResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InventoryDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = inventoryService.GetById(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<Inventory, InventoryResponse>(inventory);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductForStockIn()
        {
            List<Product> products = productService.GetAllProductForStockIn();
            if (products == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<Product>, List<ProductResponse>>(products);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllStockOut()
        {
            List<StockOut> products = stockOutService.GetAll();
            if (products == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<StockOut>, List<StockOutResponse>>(products);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllStockOutChallanList()
        {
            List<string> products = stockOutService.GetAllStockChallan();
            if (products == null)
            {
                return HttpNotFound();
            }
           // var result = AutoMapper.Mapper.Map<List<StockOut>, List<StockOutResponse>>(products);
            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}