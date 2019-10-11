using AutoMapper;
using REX_MVC.Models;
using REX_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REX_MVC.Util
{
    public class AutoMapperHelper
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserInfo, UserInfoResponse>().ReverseMap();
                cfg.CreateMap<Category, CategoryResponse>().ReverseMap();
                cfg.CreateMap<Product, ProductResponse>().ReverseMap();
                cfg.CreateMap<UserRole, UserRoleResponse>().ReverseMap();
                cfg.CreateMap<Brand, BrandResponse>().ReverseMap();
                cfg.CreateMap<Supplier, SupplierResponse>().ReverseMap();
                cfg.CreateMap<WareHouse, WareHouseResponse>().ReverseMap();
                cfg.CreateMap<Inventory, InventoryResponse>().ReverseMap();
                cfg.CreateMap<SalesMaster, SalesMasterResponse>().ReverseMap();
                cfg.CreateMap<SalesDetail, SalesDetailResponse>().ReverseMap();
                cfg.CreateMap<SalesOrder, SalesOrderResponse>().ReverseMap();
                cfg.CreateMap<ReceiveMaster, ReceiveMasterResponse>().ReverseMap();
                cfg.CreateMap<ReceiveDetail, ReceiveDetailResponse>().ReverseMap();
                cfg.CreateMap<AccountGroup, AccountGroupResponse>().ReverseMap();
                cfg.CreateMap<AccountLedger, AccountLedgerResponse>().ReverseMap();
                cfg.CreateMap<Customer, CustomerResponse>().ReverseMap();
                cfg.CreateMap<StockOut, StockOutResponse>().ReverseMap();
                cfg.CreateMap<LedgerPosting, LedgerPostingResponse>().ReverseMap();

                cfg.CreateMap<JournalMaster, JournalMasterResponse>().ReverseMap();
                cfg.CreateMap<JournalDetail, JournalDetailsResponse>().ReverseMap();
                cfg.CreateMap<rptCustomerLedger_Result, CustomerLedgerResultResponse>().ReverseMap();
                cfg.CreateMap<TempSalesMaster, TempSalesMasterResponse>().ReverseMap();
                cfg.CreateMap<TempSalesDetail, TempSalesDetailsResponse>().ReverseMap();
                cfg.CreateMap<Design, DesignResponse>()
                 .ForMember(x => x.Id,
                              m => m.MapFrom(a => a.Id))
                   .ForMember(x => x.DesignName,
                              m => m.MapFrom(a => a.DesignName)).ReverseMap();
                });
        }
    }
}