var details = [];
var detailsExpense = [];
$(document).ready(function () {
    LoadInvoiceNo("txtPoNo");
    LoadAllProduct(1, 'ddlItem');
    LoadAllWareHouse("ddlWareHouse");
    LoadPaddySupplier("ddlSupplier");
    var templateWithData = Mustache.to_html($("#templateProductModalAdd").html(), { ProductSearchAdd: details });
    $("#div-product-add").empty().html(templateWithData);

    var templateWithExpense = Mustache.to_html($("#templateExpenseModalAdd").html(), { ExpenseSearchAdd: detailsExpense });
    $("#div-expense-add").empty().html(templateWithExpense);

    Calculation();
});
function Calculation()
{
    var amount = $("#txtAmount").val();
    if (amount == '') {
        amount = 0;
    }
    var totalQty = parseFloat($("#txtTotalQty").val());
    var totalAmount = parseFloat(totalQty * amount);
    $("#txtTotalAmount").val(totalAmount.toFixed(2));
}

$("#txtAmount").on("propertychange change keyup paste input", function () {
    // do stuff;
    Calculation();
});

$("#txtDiscount").on("propertychange change keyup paste input", function () {
    GrandTotal();
});
var countAddedProduct = 1;
$("#btnAdd").click(function () {
    var countAddedProductCount = countAddedProduct++;
    var Id = $("#ddlItem option:selected").val();
    var item = $("#ddlItem option:selected").text();
    var ProductId = $("#ddlItem option:selected").val();
    //var ProductId = 6;
    var WarehouseId = $("#ddlWareHouse option:selected").val();
    var Quantity = $("#txtTotalQty").val();
    var rate = $("#txtAmount").val();
    var TotalAmount = $("#txtTotalAmount").val();
    if (WarehouseId <= 0) {
        ShowNotification("3", "Select a godown!!");
        return;
    }
    if (ProductId <= 0) {
        ShowNotification("3", "Select a product!!");
        return;
    }
    if (rate <= 0) {
        ShowNotification("3", "amount Emplty!!");
        return;
    }
    var productA = {};
    allproduct.forEach((product) => {
        if (product.Id == Id) {
                productA = product;
            }
    });
    //var object = {Id:Id, Item: item, BaleQty: BaleQty, QtyPerBale: QtyPerBale, UnitStyle: UnitStyle, Amount: Amount, TotalKg: TotalKg, TotalQty: TotalQty, TotalAmount: TotalAmount };
    var tax=TotalAmount/productA.TaxRate;
    var sd=TotalAmount/productA.SDRate
    var amount=parseFloat(TotalAmount)+ tax+sd;
   // TotalAmount += parseFloat(sd);
    var object = { countAddedProductCount: countAddedProductCount, Id: Id, Item: item, ProductId: ProductId, WarehouseId: WarehouseId, Amount: rate, Quantity: Quantity, TaxRate: tax, SDRate: sd, TotalAmount: amount };
    details.push(object);
    var templateWithData = Mustache.to_html($("#templateProductModalAdd").html(), { ProductSearchAdd: details });
    $("#div-product-add").empty().html(templateWithData);
    CalculateSum();
   // GrandTotal();   
    $("#txtTotalQty").val("0");
    $("#txtAmount").val("0");
    $("txtTotalAmount").val("0");
});

function CalculateSum()
{
    var TotalAmount = 0.0, TotalQty = 0.0;
    try {

        for (var i = 0; i < details.length; i++) {
            TotalAmount += parseFloat(details[i].TotalAmount);
            TotalQty += parseFloat(details[i].Quantity);
        }
        $("#lblTotalAmount").html(TotalAmount.toFixed(2));
        $("#lblTotalQty").html(TotalQty.toFixed(2));
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
   // GrandTotal();//update Grand Total
}

function GetDataFromDataTable(productId) {
    var obj = new Object();
    $('#productGroupTableModal tr').each(function (i) {
        if (i > 0) {
            var Id = $(this).find('td').eq(0).text();
            if (productId==Id) {
                var ItemInfo = $(this).find('td').eq(1).text();
                var Barcode1 = $(this).find('td').eq(2).text();
                var RetailPrice = $(this).find('td').eq(3).text();
                var WholeSalesPrice = $(this).find('td').eq(4).text();
                var Quantity = $(this).find('td').eq(5).find('input').val();
                var taxRate = $(this).find('td').eq(6).find('input').val();
                var SDRate = $(this).find('td').eq(7).find('input').val();
              
                obj.ItemId = Id;
                obj.ItemInfo = ItemInfo;
                obj.Barcode1 = Barcode1;
                obj.CostPrice = 0;
                obj.RetailPrice = RetailPrice;
                obj.WholeSalesPrice = WholeSalesPrice;
                obj.Qty = Quantity;
                obj.TaxRate = taxRate;
                obj.SDRate = SDRate;
                details.push(obj);
                var templateWithData = Mustache.to_html($("#templateProductModalAdd").html(), { ProductSearchAdd: details });
                $("#div-product-add").empty().html(templateWithData);
               // MakePagination('productTableModalAdded');
            } 
        }
    });
    return details;
}

function Save()
{
    console.log($("#ddlWareHouse").val());
    var supplierId = $("#ddlSupplier").val();
    var godown = $("#ddlWareHouse").val();
    if (details.length <= 0) {
        ShowNotification("3", "Not added any Item!!");
        return;
    }
    if (supplierId <= 0) {
        ShowNotification("3", "Select a Supplier!!");
        return;
    }
    if (godown <= 0) {
        ShowNotification("3", "select a godown!!");
        return;
    }
    //debugger;
    $("#btnSave").prop("disabled", true);
    var url = '/Purchase/Save';
    $.ajax({
        url: url,
        method: 'POST',
        data: {          
            PONo: $("#txtPoNo").val(),
            totalAmount: $("#lblTotalAmount").text(),
            supplierId: supplierId,
            descriptions: $("#txtDescriptions").val(),
            WarehouseId: godown,
            dates: $("#txtDates").val(),
            Discount: $("#txtDiscount").val(),
            response: details,
            ledgerPosting: detailsExpense
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
            location.reload();
        },
        error: function () {
        }
    });
}
function LoadInvoiceNo(controlId) {
    var url = "/Purchase/GetInvoiceNumber";
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

function LoadPaddySupplier(controlId) {
    var url = '/Suppliers/GetAllPaddySupplier';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var data = res;
            //alert('Success');
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (true) {
                $("#" + controlId).get(0).options[0] = new Option("-Select-", "");
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
