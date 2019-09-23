﻿var details = [];
var detailsExpense = [];
$(document).ready(function () {
    LoadInvoiceNo("txtPoNo");
    LoadAllProduct();
    LoadAllWareHouse("ddlWareHouse");
    LoadLocalSupplier("ddlSupplier");
    var templateWithData = Mustache.to_html($("#templateProductModalAdd").html(), { ProductSearchAdd: details });
    $("#div-product-add").empty().html(templateWithData);

    var templateWithExpense = Mustache.to_html($("#templateExpenseModalAdd").html(), { ExpenseSearchAdd: detailsExpense });
    $("#div-expense-add").empty().html(templateWithExpense);

    Calculation();
    CalculationExpense();
});

function Calculation()
{
    var BaleQty = $("#txtBaleQty").val();
    var QtyPerBale = $("#txtQtyPerBale").val();
    var UnitStyle = $("#txtUnitStyle").val();
    var amount = $("#txtAmount").val();
    if (BaleQty=='') {
        BaleQty = 0;
    }
    if (QtyPerBale == '') {
        QtyPerBale = 0;
    }
    if (UnitStyle == '') {
        UnitStyle = 1;
    }
    if (amount == '') {
        amount = 0;
    }
    var totalInKG = QtyPerBale * BaleQty;
    $("#txtTotalinKG").val(totalInKG);
    var totalQty = totalInKG / UnitStyle;
    $("#txtTotalQty").val(totalQty);
    var totalAmount = totalQty * amount;
    $("#txtTotalAmount").val(totalAmount);
}

function CalculationExpense() {
    var ExpenseRate = $("#txtExpenseRate").val();
    var ExpenseAmount = $("#txtExpenseAmount").val();
    if (ExpenseRate == '') {
        ExpenseRate = 0;
        $("#txtExpenseRate").val('0');
    }
    if (ExpenseAmount == '') {
        ExpenseAmount = 0;
        $("#txtExpenseAmount").val('0');
    }
    var totalExpenseAmount = ExpenseRate * ExpenseAmount;
    $("#txtExpenseTotalAmount").val(totalExpenseAmount);
}

$("#txtBaleQty").on("propertychange change keyup paste input", function () {
    // do stuff;
    Calculation();
});
$("#txtQtyPerBale").on("propertychange change keyup paste input", function () {
    // do stuff;
    Calculation();
});
$("#txtUnitStyle").on("propertychange change keyup paste input", function () {
    // do stuff;
    Calculation();
});
$("#txtAmount").on("propertychange change keyup paste input", function () {
    // do stuff;
    Calculation();
});

$("#txtExpenseRate").on("propertychange change keyup paste input", function () {
    // do stuff;
    CalculationExpense();
});
$("#txtExpenseAmount").on("propertychange change keyup paste input", function () {
    // do stuff;
    CalculationExpense();
});

var countAddedProduct = 1;
$("#btnAdd").click(function () {
    var countAddedProductCount = countAddedProduct++;

    var Id = $("#ddlItem option:selected").val();
    var item = $("#ddlItem option:selected").text();
    var ProductId = $("#ddlItem option:selected").val();
    //var ProductId = 6;
    var WarehouseId = $("#ddlWareHouse option:selected").val();
    //var WarehouseId = 1;
    var TotalBale = $("#txtBaleQty").val();
    var QtyInBale = $("#txtQtyPerBale").val();
    var WeightInKG = $("#txtTotalinKG").val();
    var WeightType = $("#txtUnitStyle").val();
    var WeightInMon = $("#txtTotalQty").val();
    var Amount = $("#txtAmount").val();
    var TotalAmount = $("#txtTotalAmount").val();
    if (WarehouseId <= 0) {
        ShowNotification("3", "Select a godown!!");
        return;
    }
    if (ProductId <= 0) {
        ShowNotification("3", "Select a product!!");
        return;
    }
    if (WeightInKG <= 0) {
        ShowNotification("3", "qty Emplty!!");
        return;
    }
    if (Amount <= 0) {
        ShowNotification("3", "amount Emplty!!");
        return;
    }
    //var object = {Id:Id, Item: item, BaleQty: BaleQty, QtyPerBale: QtyPerBale, UnitStyle: UnitStyle, Amount: Amount, TotalKg: TotalKg, TotalQty: TotalQty, TotalAmount: TotalAmount };
    var object = { countAddedProductCount: countAddedProductCount, Id: Id, Item: item, ProductId: ProductId, WarehouseId: WarehouseId, TotalBale: TotalBale, QtyInBale: QtyInBale, WeightInKG: WeightInKG, WeightType: WeightType, WeightInMon: WeightInMon, Amount: Amount, TotalQty: WeightInMon, TotalAmount: TotalAmount };
    details.push(object);
    var templateWithData = Mustache.to_html($("#templateProductModalAdd").html(), { ProductSearchAdd: details });
    $("#div-product-add").empty().html(templateWithData);
    CalculateSum();
    GrandTotal();
     $("#txtBaleQty").val('0');
     $("#txtQtyPerBale").val('0');
     $("#txtTotalinKG").val('0');
     $("#txtUnitStyle").val('0');
     $("#txtTotalQty").val('0');
     $("#txtAmount").val('0');
     $("#txtTotalAmount").val('0');
});

var countAddedExpense = 1;
$("#btnAddExpense").click(function () {
    var countAddedExpenseCount = countAddedExpense++;
    var Id = $("#ddlExpenseHead option:selected").val();
    var item = $("#ddlExpenseHead option:selected").text();
    var ledgerId = $("#ddlExpenseHead option:selected").val();
    var ExpenseRate = $("#txtExpenseRate").val();
    var ExpenseAmount = $("#txtExpenseAmount").val();
    var TotalAmount = ExpenseRate * ExpenseAmount;
    $("#txtExpenseTotalAmount").val(TotalAmount);
    var object = { countAddedExpenseCount: countAddedExpenseCount, Id: Id, Item: item, ExpenseRate: ExpenseRate, ExpenseAmount: ExpenseAmount, TotalAmount: TotalAmount, Debit: TotalAmount, LedgerId: ledgerId };
    detailsExpense.push(object);
    //expense box

    var templateWithExpense = Mustache.to_html($("#templateExpenseModalAdd").html(), { ExpenseSearchAdd: detailsExpense });
    $("#div-expense-add").empty().html(templateWithExpense);
    CalculateExpenseSum();
    GrandTotal();
    $("#txtExpenseRate").val("0");
    $("#txtExpenseAmount").val("0");
    $("#txtExpenseTotalAmount").val("0");
});


function CalculateSum()
{
    var TotalAmount = 0.0, TotalQty = 0.0;
    try {

        for (var i = 0; i < details.length; i++) {
            console.log(details[i].TotalQty);
            TotalAmount += parseFloat(details[i].TotalAmount);
            TotalQty += parseFloat(details[i].TotalQty);     
        }
        $("#lblTotalAmount").html(TotalAmount);
        $("#lblTotalQty").html(TotalQty);

    } catch (e) {
        console.log(e);

    }
}

//Calculate Total of Aditional Expense
function CalculateExpenseSum() {
    var TotalAmount = 0.0;
    try {

        for (var i = 0; i < detailsExpense.length; i++) {
            console.log(detailsExpense[i].TotalAmount);
            TotalAmount += parseFloat(detailsExpense[i].TotalAmount);
        }
        $("#lblTotalExpenseAmount").html(TotalAmount.toFixed(2));

    } catch (e) {
        console.log(e);

    }
}
// Calculating Grand Total
function GrandTotal() {
    try {
        var GTotal = 0.0;
        $("#lblGrandTotal").html(GTotal.toFixed(2));
        var TotalAdded = parseFloat($('#lblTotalAmount').text());
        var TotalExpense = parseFloat($('#lblTotalExpenseAmount').text());
        GTotal = TotalAdded + TotalExpense;
        console.log(GTotal);
        $("#lblGrandTotal").html(GTotal.toFixed(2));
    } catch (e) {
        console.log(e);
    }
}

function OnDeleteProduct(productId)
{
    for (var i = 0; i < details.length; i++) {
        if (details[i].countAddedProductCount == productId)
        {
            details.splice(i, 1);
        }
    }
    var templateWithData = Mustache.to_html($("#templateProductModalAdd").html(), { ProductSearchAdd: details });
    $("#div-product-add").empty().html(templateWithData);
    CalculateSum();
    GrandTotal();
}

function OnDeleteExpense(expenseId) {
    for (var i = 0; i < detailsExpense.length; i++) {
        if (detailsExpense[i].countAddedExpenseCount == expenseId) {
            detailsExpense.splice(i, 1);
        }
    }
    var templateWithExpense = Mustache.to_html($("#templateExpenseModalAdd").html(), { ExpenseSearchAdd: detailsExpense });
    $("#div-expense-add").empty().html(templateWithExpense);
    CalculateExpenseSum();
    GrandTotal();
}

function GetDataFromDataTable(productId) {
    $('#productGroupTableModal tr').each(function (i) {
        if (i > 0) {
            var Id = $(this).find('td').eq(0).text();
            if (productId==Id) {
                var ItemInfo = $(this).find('td').eq(1).text();
                var Barcode1 = $(this).find('td').eq(2).text();
                var RetailPrice = $(this).find('td').eq(3).text();
                var WholeSalesPrice = $(this).find('td').eq(4).text();
                var Quantity = $(this).find('td').eq(5).find('input').val();
                var obj = new Object();
                obj.ItemId = Id;
                obj.ItemInfo = ItemInfo;
                obj.Barcode1 = Barcode1;
                obj.CostPrice = 0;
                obj.RetailPrice = RetailPrice;
                obj.WholeSalesPrice = WholeSalesPrice;
                obj.Qty = Quantity;
                details.push(obj);
                var templateWithData = Mustache.to_html($("#templateProductModalAdd").html(), { ProductSearchAdd: details });
                $("#div-product-add").empty().html(templateWithData);
               // MakePagination('productTableModalAdded');
            } 
        }
    });

    return details;
}

function SaveForLocalMarket() {
    var url = '/GoodsReceive/SaveForLocalMarket';
    $.ajax({
        url: url,
        method: 'POST',
        data: {
            totalAmount: $("#lblTotalAmount").text(),
            PONo: $("#txtPoNo").val(),
            supplierId: $("#ddlSupplier").val(),
            descriptions: $("#txtDescriptions").val(), 
            WarehouseId: $("#ddlWareHouse").val(),
            dates:$("#txtDates").val(),
            response: details,
            additionalCost: detailsExpense
        },
        success: function (data) {
            //debugger;
            //var GRNo = data.ID;
            //var param = "ReportName=GrChallanReport&GRNo=" + GRNo;
            //window.open("../Report/Viewer/ReportViewer.aspx?" + param, "_blank");
            setTimeout(location.reload.bind(location), 10000);
            ShowNotification("1", "Item Recived Saved!!");
            details = [];
            detailsExpense = [];
            var templateWithData = Mustache.to_html($("#templateProductModalAdd").html(), { ProductSearchAdd: details });
            $("#div-product-add").empty().html(templateWithData);
            var templateWithExpense2 = Mustache.to_html($("#templateExpenseModalAdd").html(), { ExpenseSearchAdd: detailsExpense });
            $("#div-expense-add").empty().html(templateWithExpense2);
            LoadInvoiceNo("txtPoNo");
        },
        error: function () {
        }
    });

}

function LoadAllProduct() {
    var url = "/Product/GetAll?Type=3";
    $.ajax({
        url: url,
        method: "POST",
        success: function (res) {
            var data = res;
            //alert('Success');
            var controlId = "ddlItem";
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (true) {
                $("#" + controlId).get(0).options[0] = new Option("-পছন্দ করুন-", "-1");
            }
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.ProductName, item.Id);
                });
            }
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!" });

        },
        error: function () {
        }
    });
    
}

function LoadInvoiceNo(controlId) {
    var url = "/GoodsReceive/GetInvoiceNumberForLocal";
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

function LoadLocalSupplier(controlId) {
    var url = '/Suppliers/GetAllRiceSupplier';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var data = res;
            //alert('Success');
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (true) {
                $("#" + controlId).get(0).options[0] = new Option("-পছন্দ করুন-", "");
            }
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option('' + item.Code + '-' + item.Name, item.Id);
                });
            }
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!", search_contains: true });
        },
        error: function () {
        }
    });
}





