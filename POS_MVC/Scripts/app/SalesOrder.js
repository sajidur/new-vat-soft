var detailsSalesOrder = [];
var detailSalesOrderForPost = [];

$(document).ready(function () {
    LoadInvoiceNo("txtPoNo");
    LoadCustomerCombo("ddlCustomer");
    LoadProductList();
    Expression();
});

function Expression() {
    var BaleWight = $("#txtBaleWeight");
    BaleWight.keyup(function () {
        CalculationFirst();
    });
}
function CalculationFirst() {
    var BaleQty = $("#txtBaleQty").val();
    var BaleWight = $("#txtBaleWeight").val();
    var totalKG = parseFloat(BaleQty * BaleWight);
    var Mon = parseFloat(totalKG / 40);
    $("txtWeightInMon").html(Mon.toFixed(2));
}

function Calculation() {
    var totalBaleQty = 0;
    var totalQtyKG = 0
    var totalAmount = 0;
    for (var i = 0; i < detailsSalesOrder.length; i++) {
        totalBaleQty += detailsSalesOrder[i].BaleQty;
        totalQtyKG += detailsSalesOrder[i].TotalQtyInKG;
        totalAmount += detailsSalesOrder[i].Amount;
    }
    
    $("#lblTotalQty").html(totalBaleQty);
    $("#lblTotalAmount").html(totalAmount);
}
function LoadInvoiceNo(controlId) {
    var url = "/Sales/GetInvoiceNumberSalesOrder";
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

function LoadProductList() {
    var url = '/ProductionProcessing/GetProductForStockIn';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var templateWithData = Mustache.to_html($("#templateSalesOrderGroupModal").html(), { SalesOrderGroupSearch: res });
            $("#div-salesOrderGroup").empty().html(templateWithData);
            MakePagination('salesOrderGroupTableModal');
        },
        error: function (error, r) {
            ShowNotification("3", "Something Wrong!!");
        }
    });
}

var count = 1;
function LoadForAdd(parameters) {
    $.ajax({
        url: '/Product/Details',
        data: { 'id': parameters },
        success: function (data) {
            var countCount = count++;
            var Id = data.Id;
            var ProductName = data.ProductName;
            var BaleQty = 0;
            var BaleWeight = 0;
            var Rate = 0;
            var Notes='';
            $('#salesOrderGroupTableModal tr').each(function (i) {
                if ($(this).find('td').eq(0).text() == Id) {
                    BaleQty = $(this).find('td').eq(2).find('input').val();
                    BaleWeight = $(this).find('td').eq(3).find('input').val();
                    Rate = $(this).find('td').eq(5).find('input').val();
                    Notes = $(this).find('td').eq(1).text();
                }
            });  
            var TotalQtyInKG = BaleQty * BaleWeight;            
            var Amount = Rate * BaleQty;
            var object = {
                countCount: countCount,
                Id: Id,
                ProductName: ProductName,
                BaleQty: BaleQty, 
                BaleWeight: BaleWeight,
                TotalQtyInKG: TotalQtyInKG, 
                Rate: Rate,
                Amount: Amount,
                Notes: ProductName
            };
            detailsSalesOrder.push(object);
            var templateWithData = Mustache.to_html($("#templateSalesOrderGroupModalGrid").html(), { SalesOrderGroupSearchGrid: detailsSalesOrder });
            $("#div-salesOrderGroupGrid").empty().html(templateWithData);
            Calculation();
        },
        error: function () {
            alert('An error occured try again later');
        }
    });
}


function Save() {
    GetDataFromDatatable();
    var url = '/Sales/SaveSalesOrder';
    console.log(detailSalesOrderForPost);
    $("#btnSave").prop("disabled", true);
    $.ajax({
        url: url,
        method: 'POST',
        data: {
            salesOrders: detailSalesOrderForPost,
            isSendSMS: $('#chkSendSMS').is(':checked')
        },
        success: function (data) {
            setTimeout(location.reload.bind(location), 10000);
            ShowNotification("1", "Sales Order Saved!!");
            detailsSalesOrder=[];
            var templateWithData = Mustache.to_html($("#templateSalesOrderGroupModalGrid").html(), { SalesOrderGroupSearchGrid: detailsSalesOrder });
            $("#div-salesOrderGroupGrid").empty().html(templateWithData);
        },
        error: function () {
        }
    });

}

function GetDataFromDatatable() {
    $('#salesOrderGroupTableModalGrid tr').each(function (i) {
        if (i > 0) {
            var ProductId = $(this).find('td').eq(1).text();
            var ProductName = $(this).find('td').eq(2).text();
            var BaleQty = $(this).find('td').eq(3).text();
            var BaleWeight = $(this).find('td').eq(4).text();
            var TotalQtyInKG = $(this).find('td').eq(5).text();
            var Rate = $(this).find('td').eq(6).text();
            var Amount = $(this).find('td').eq(7).text();
            var DeliveryDate = $("#txtDate").val();
            var Notes = $("#txtDescriptions").val();
            var CustomerID = $("#ddlCustomer option:selected").val();
            var SalesOrderId = $("#txtPoNo").val();
            var product = {
                ProductId: ProductId,
                ProductName: ProductName
            }
            var object = {
                ProductId: ProductId,
                BaleQty: BaleQty,
                BaleWeight: BaleWeight,
                TotalQtyInKG: TotalQtyInKG,
                Rate: Rate,
                Amount: Amount,
                DeliveryDate: DeliveryDate,
                Notes: Notes,
                CustomerID: CustomerID,
                SalesOrderId: SalesOrderId,
                Product: product
            };
            detailSalesOrderForPost.push(object);

        }
    });
}


function OnDeleteSalesOrder(Id) {
    for (var i = 0; i < detailsSalesOrder.length; i++) {
        if (detailsSalesOrder[i].countCount == Id) {
            detailsSalesOrder.splice(i, 1);
        }
    }
    var templateWithData = Mustache.to_html($("#templateSalesOrderGroupModalGrid").html(), { SalesOrderGroupSearchGrid: detailsSalesOrder });
    $("#div-salesOrderGroupGrid").empty().html(templateWithData);
    Calculation();
}