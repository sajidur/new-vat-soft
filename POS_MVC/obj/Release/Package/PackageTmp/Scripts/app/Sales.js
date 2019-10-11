
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

var count = 1;
function LoadForAdd(InvId) {
    var countCount = count++;
    var Id = "0";
    var ProductName = "";
    var Qty = '0';
    var BaleWeight = '0';
    var Rate = '';
    var ProductId = '0';
    var SDRate = 0;
    var TaxRate = 0;
    $('#inventoryGroupTableModal tr').each(function (i) {
        if ($(this).find('td').eq(0).text() == InvId) {
            Id = $(this).find('td').eq(1).html();
            ProductName = $(this).find('td').eq(2).html();
            ProductId = Id;
            Qty = $(this).find('td').eq(6).find('input').val();
            Rate = $(this).find('td').eq(7).find('input').val();
            SDRate = $(this).find('td').eq(8).html();
            TaxRate = $(this).find('td').eq(9).html();
            WarehouseId = $(this).find('td').eq(9).text();
        }
    });
    var Amount = Rate * Qty;
    var SalesOrderId = '';

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
    var sd=Amount / SDRate;
    var tax=Amount/TaxRate;
    var totalamount=Amount+sd+tax;
    var object = {
        countCount: countCount,
        Id: Id,
        ProductId:Id,
        ProductName: ProductName,
        Qty: Qty,
        Rate: Rate,
        SD: sd,
        Tax:tax,
        Amount: totalamount,
        SalesOrderId: SalesOrderId,
        WarehouseId: WarehouseId
    };
    detailsSales.push(object);
    var templateWithData = Mustache.to_html($("#templateSalesGroupModalGrid").html(), { SalesGroupSearchGrid: detailsSales });
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
        if (detailsSales.length <= 0) {
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
                    salesDetail: detailsSales,
                    Discount: $("#txtDiscount").val(),
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
                var TotalAmount = $("#lblTotalAmountA").html();
                var Notes = $("#txtDescriptions").val();
                var Coutha = $("#txtCoutha").val();
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
                    DiscountPurpose: $("#txtDiscountNotes").val(),
                    TransportType: $("#txtDelivery").val(),
                    TransportNo: $("#txtDriverMob").val()
                };
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
