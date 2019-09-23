
var detailsSales = [];
var detailsSalesForPost = [];
var detailSalesForPost = [];
var detailSalesInMaster = [];
var orderElements = [];
var orderDeliveryQty = [];
var creditLimit = 0;
var balance=0;
//function GetCustomerId() {

//}

function LoadInvoiceNo(controlId) {
    var url = "/Sales/GetInvoiceNumber";
    $.ajax({
        url: url,
        method: "POST",
        success: function (res) {
            var data = res;
            console.log(data);
            $("#" + controlId).val(data);
        },
        error: function () {
        }
    });
}


function LoadInventoryList() {
    var url = '/Sales/GetAllInventoryforSales';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var templateWithData = Mustache.to_html($("#templateInventoryGroupModal").html(), { InventoryGroupSearch: res });
            $("#div-inventoryGroup").empty().html(templateWithData);
            MakePagination('inventoryGroupTableModal');
        },
        error: function (error, r) {
            ShowNotification("3", "Something Wrong!!");
        }
    });
}

function LoadSalesOrderList() {
    var CustomerID = 0;
    var fromDate = new Date().toISOString();
    var toDate = new Date().toISOString();
    var customerDropdown = document.getElementById("ddlCustomer");
    if (customerDropdown.selectedIndex == -1) {
        CustomerID = 0;
    }
    else {
        var customerId = customerDropdown.options[customerDropdown.selectedIndex].value;
        CustomerID = customerId;
    }
    var url = '/Sales/GetSaleOrdersFilter';
    $.ajax({
        url: url,
        method: 'GET',
        data: { 'CustomerID': CustomerID, 'fromDate': fromDate, 'toDate': toDate },
        success: function (res) {
            creditLimit = res.Limit;
            balance=res.Balance;
            console.log(res);
            $("#txtCoutha").val(res.chouthaNo);
            $("#lblBalance").text(" Balance:"+res.Balance+" and Credit Limit: "+res.Limit);
            var templateWithData = Mustache.to_html($("#templateSalesOrderGroupModal").html(), { SalesOrderGroupSearch: res.salesOrders });
            $("#div-salesOrderGroup").empty().html(templateWithData);
            MakePagination('salesOrderGroupTableModal');
        },
        error: function (error, r) {
            ShowNotification("3", "Something Wrong!!");
        }
    });

   // LoadAllWareHouse("ddlWareHouse");
}

var count = 1;
function LoadForAdd(InvId) {
    var countCount = count++;
    var Id = "0";
    var ProductName = "";
    var BaleQty = '0';
    var BaleWeight = '0';
    var Rate = '';
    var ProductId = '0';
    $('#inventoryGroupTableModal tr').each(function (i) {
        if ($(this).find('td').eq(0).text() == InvId) {
            Id = $(this).find('td').eq(1).html();
            ProductName = $(this).find('td').eq(2).html();
            ProductId = Id;
            BaleQty = $(this).find('td').eq(6).find('input').val();
            BaleWeight = $(this).find('td').eq(3).text();
            Rate = $(this).find('td').eq(7).find('input').val();
            WarehouseId = $(this).find('td').eq(9).text();
        }
    });
    var TotalQtyInKG = BaleQty * BaleWeight;
    var Amount = Rate * BaleQty;
    var SalesOrderId = '';
    if (TotalQtyInKG <= 0) {
        ShowNotification("3", "Qty empty!!");
        return;
    }
    if (Amount <= 0) {
        ShowNotification("3", "Amount empty!!");
        return;
    }
    if (ProductId <= 0) {
        ShowNotification("3", "Select a product!!");
        return;
    }
    if (WarehouseId <= 0) {
        ShowNotification("3", "Select godown!!");
        return;
    }
    var object = {
        countCount: countCount,
        Id: Id,
        ProductId:Id,
        ProductName: ProductName,
        BaleQty: BaleQty,
        BaleWeight: BaleWeight,
        TotalQtyInKG: TotalQtyInKG, Rate: Rate,
        Amount: Amount,
        SalesOrderId: SalesOrderId,
        WarehouseId: WarehouseId
    };
    console.log(object);
    detailsSales.push(object);
    var templateWithData = Mustache.to_html($("#templateSalesGroupModalGrid").html(), { SalesGroupSearchGrid: detailsSales });
    $("#div-sales-add").empty().html(templateWithData);
    Calculation();
    ShowNotification("1", "Item added!!");

}

function LoadForAddOrder(parameters) {
    var countCount = count++;
    var Id = '';
    var ProductName = '';
    var ProductId = '';
    var OrderQty = '';
    var BaleQty = '';
    var BaleWeight = '';
    var Rate = '';
    var SalesOrderId = '';
    var IsActive;       

        $('#salesOrderGroupTableModal tr').each(function (i) {
            if ($(this).find('td').eq(0).text() == parameters) {
                Id = $(this).find('td').eq(1).text();
                ProductName = $(this).find('td').eq(2).text();
                OrderQty = $(this).find('td').eq(3).text();
                BaleQty = $(this).find('td').eq(6).find('input').val();
                BaleWeight = $(this).find('td').eq(4).text();
                Rate = $(this).find('td').eq(7).find('input').val();
                SalesOrderId = $(this).find('td').eq(0).text();
                ProductId = $(this).find('td').eq(11).text();
                if ($(this).find('td').eq(8).find('input').is(":checked"))
                {
                    IsActive = true;
                    console.log(IsActive);
                } else {
                    IsActive = false;
                    console.log(IsActive);
                }
                
                WarehouseId = 1;
            }
        });
        
        if (parseFloat(OrderQty) == parseFloat(BaleQty)) {
            IsActive = false;
            //console.log(OrderQty);
            //console.log(BaleQty);
            console.log(IsActive);
        } else if (IsActive == true) {
            IsActive = false;
            console.log(IsActive);
        } else {
            IsActive = true;
            console.log(IsActive);
        }
        var TotalQtyInKG = BaleQty * BaleWeight;
        var Amount = Rate * BaleQty;
        var object = {
            countCount: countCount,
            Id: Id,
            ProductId:ProductId,
            ProductName: ProductName,
            BaleQty: BaleQty,
            BaleWeight: BaleWeight,
            TotalQtyInKG: TotalQtyInKG,
            Rate: Rate,
            Amount: Amount,
            SalesOrderId: SalesOrderId,
            IsActive: IsActive,
            WarehouseId: WarehouseId
        };
      
        var object2 = {
            BaleQty: BaleQty
        };
        console.log(object);
        orderDeliveryQty.push(object);
        orderElements.push(object);
        detailsSales.push(object);
        detailSalesInMaster.push(object);
        var templateWithData = Mustache.to_html($("#templateSalesGroupModalGrid").html(), { SalesGroupSearchGrid: detailSalesInMaster });
        $("#div-sales-add").empty().html(templateWithData);
        Calculation();
        ShowNotification("1", "Item added!!");

    }

    function Calculation() {

        var totalAmount = 0;
        for (var i = 0; i < detailsSales.length; i++) {
            totalAmount += detailsSales[i].Amount;
        }

        $("#lblTotalAmountA").html(totalAmount);
        GrandTotal();
    }

    function GrandTotal() {
        var discount = $("#txtDiscount").val();
        var totalAmount = parseFloat($("#lblTotalAmountA").text());
        if (discount != 0) {
            var GTotal = totalAmount - discount;
            $("#lblGTotal").html(GTotal.toFixed(2));
        } else { $("#lblGTotal").html(totalAmount); }
    }

    function Expression() { 
        $("#txtDiscount").on("propertychange change keyup paste input", function () {
            GrandTotal();
        });
    }
    function ToJavaScriptDate(value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
    }

    function Save(e) {
        GetDataFromDatatable();
        var IsActive;
        if (detailSalesInMaster.length <= 0) {
            ShowNotification("3", "No Item added!!");
            return;
        }
        else
        {
            $("#btnSave").prop("disabled", true);
        }

        var totalCredit =Math.abs(balance - parseFloat($("#lblGTotal").text()));
        var r = true;
        if (creditLimit != 0 && totalCredit > creditLimit) {
            r = confirm("Already Limit Over! বাকির সীমা অতিক্রম করেছে ,আপনি কি আরো বাকি দিতে চান? ");
        }
        if (r == true) {

            var url = '/Sales/SaveSales';
            $.ajax({
                url: url,
                method: 'POST',
                data: {
                    salesMasters: detailsSalesForPost,
                    salesDetail: detailSalesInMaster,
                    salesOrders: orderElements,
                    lstDeliveryQunatities: orderDeliveryQty,
                    Discount: $("#txtDiscount").val(),
                    isSendSMS: false,
                    DriverName: $("#txtDriverName").val(),
                    RentAmount: $("#txtRentAmount").val()

                },
                success: function (data) {
                    ShowNotification("1", "Sales Saved!!");
                    var templateWithData = Mustache.to_html($("#templateSalesGroupModalGrid").html(), { SalesGroupSearchGrid: detailsSales });
                    $("#div-sales-add").empty().html(templateWithData);
                    // window.location.href = ;
                    setTimeout(location.reload.bind(location), 10000);
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }

    }


    function GetDataFromDatatable() {
        detailSalesInMaster = [];
        detailsSalesForPost = [];
        $('#salesGroupTableModalGrid tr').each(function (i) {
            if (i > 0) {
                var SalesInvoice = $("#txtPoNo").val();
                var CustomerID = $("#ddlCustomer option:selected").val();
                var TotalAmount = $(this).find('td').eq(7).text();
                var Notes = $("#txtDescriptions").val();
                var Coutha = $("#txtCoutha").val();
                var ProductId = $(this).find('td').eq(1).text();
                var BaleQty = $(this).find('td').eq(3).text();
                var BaleWeight = $(this).find('td').eq(4).text();
                var TotalQtyInKG = $(this).find('td').eq(5).text();
                var Rate = $(this).find('td').eq(6).text();
                var Amount = $(this).find('td').eq(7).text();
                var SalesOrderId = $(this).find('td').eq(8).text();
                var WarehouseId = $(this).find('td').eq(10).text();
                var SalesDate = $("#txtDate").val();

                if (WarehouseId <= 0) {
                    ShowNotification("3", "select godown!!");
                    return;
                }
                var object = {
                    SalesInvoice: SalesInvoice,
                    CustomerID: CustomerID,
                    TotalAmount: TotalAmount,
                    SalesDate:SalesDate,
                    Notes: Notes,
                    Coutha:Coutha,
                    SalesOrderId: SalesOrderId,
                    DiscountPurpose: $("#txtDiscountNotes").val(),
                    TransportType: $("#txtDelivery").val(),
                    TransportNo: $("#txtDriverMob").val()
                };

                var object2 = {
                    ProductId: ProductId,
                    BaleQty: BaleQty,
                    BaleWeight: BaleWeight,
                    TotalQtyInKG: TotalQtyInKG,
                    Rate: Rate,
                    Amount: Amount,
                    WarehouseId: WarehouseId
                };
                detailSalesInMaster.push(object2);
                detailsSalesForPost.push(object);
            }
        });
    }


    function OnDeleteSalesOrder(Id) {
        for (var i = 0; i < detailsSales.length; i++) {
            if (detailsSales[i].countCount == Id) {
                detailsSales.splice(i, 1);
            }
        }
        var templateWithData = Mustache.to_html($("#templateSalesGroupModalGrid").html(), { SalesGroupSearchGrid: detailsSales });
        $("#div-sales-add").empty().html(templateWithData);
        Calculation();
        GrandTotal();
    }
