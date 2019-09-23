

var detailsStockOut = [];
var datatableRowCount = 0;

$(document).ready(function () {
    LoadInvoiceNo("txtPoNo");
    LoadAllProduct();
    LoadAllWareHouse("ddlWareHouse");
    //LoadSupplierCombo("ddlSupplier");

    LoadInventoryList();

    var templateWithData = Mustache.to_html($("#templateStockOutModal").html(), { StockTableAdd: detailsStockOut });
    $("#div-stockOut-add").empty().html(templateWithData);
});


$("#btnAdd").click(function () {
    var msg = '';
    $(this).closest('tr').find('input').each(function () {
        msg += $(this).val() + '\n';
    });
    console.log(msg);
});

function OnDeleteStockOut(Id) {
    for (var i = 0; i < detailsStockOut.length; i++) {
        if (detailsStockOut[i].inventoryCountCount == Id) {
            detailsStockOut.splice(i, 1);
        }
    }
    var templateWithData = Mustache.to_html($("#templateStockOutModal").html(), { StockTableAdd: detailsStockOut });
    $("#div-stockOut-add").empty().html(templateWithData);
}


function LoadAllProduct() {
    var url = "/Product/GetAll";
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
        },
        error: function () {
        }
    });

}

function GetStockOutReport() {
    $('#salesReportTableModal').DataTable({
        "jQueryUI": true,
        'ordering': true,
        'searching': true,
        'paging': false,
        //'order': [[0, 'desc']],
        'order': [[0, 'asc']],
        "ajax": {
            "dataType": 'json',
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "/ProductionProcessing/GetAllStockOut",
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            {
                "data": "ProductionDate",
                'render': function (jsonDate) {
                    var date = new Date(parseInt(jsonDate.substr(6)));
                    var month = date.getMonth() + 1;
                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            { "data": "InvoiceNo" },
            { "data": "BaleWeight" },
            { "data": "BaleQty" },
            { "data": "WeightInMon" },
            { "data": "Notes" }

        ],

        "bDestroy": true,
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
}


function LoadInvoiceNo(controlId) {
    var url = "/ProductionProcessing/GetInvoiceNumber";
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


function Save() {
    var url = '/ProductionProcessing/Save';
    $.ajax({
        url: url,
        method: 'POST',
        data: {
            InvoiceNo: $("#txtPoNo").val(),            
            //SupplierId: $("#ddlSupplier option:selected").val(),
            Notes: $("#txtDescriptions").val(),
            stockOuts: detailsStockOut
        },
        success: function (data) {
            ShowNotification("1", "Stock Out Saved!!");
            detailsStockOut = [];
            var templateWithData = Mustache.to_html($("#templateStockOutModal").html(), { StockTableAdd: detailsStockOut });
            $("#div-stockOut-add").empty().html(templateWithData);


            window.location.href = "/Report/Viewer/ReportViewer.aspx?ReportName=StockOutForProcessing&invoiceId=" + $("#txtPoNo").val();
           // window.open("/Report/Viewer/ReportViewer.aspx?ReportName=StockOutForProcessing&invoiceId=" + $("#txtPoNo").val(), "_blank");
        },
        error: function () {
        }
    });

}


function LoadInventoryList() {
    var url = '/ProductionProcessing/GetAllInventory';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var templateWithData = Mustache.to_html($("#templateInventoryGroupModal").html(), { InventoryGroupSearch: res });
            $("#div-inventoryGroup").empty().html(templateWithData);
            MakePagination('inventoryGroupTableModal');
        },
        error: function (error, r) {
            console.log(error);
            console.log(r.responseText);
            ShowNotification("3", "Something Wrong!!");
        }
    });
}

var inventoryCount = 1;
function LoadForAdd(parameters) {
    $.ajax({
        url: '/ProductionProcessing/InventoryDetails',
        data: { 'id': parameters },
        success: function (data) {
            var inventoryCountCount = inventoryCount++;
            var Id = data.Id;
            var ProductId = data.ProductId;
            var WarehouseId = data.WarehouseId;
            var table = $('#inventoryGroupTableModal').DataTable();            

            var BaleQty = '';
            $('#inventoryGroupTableModal tr').each(function (i) {
                if ($(this).find('td').eq(0).text() == Id) {
                    BaleQty = $(this).find('td').eq(7).find('input').val();
                }
            });


            var BaleWeight = data.QtyInBale;
            var WeightInMon = data.QtyInBale;
            var SupplierId = data.SupplierId;

            var Product = data.Product;
            var Supplier = data.Supplier;
            var WareHouse = data.WareHouse;
            
            var object = {
                inventoryCountCount : inventoryCountCount,
                Id: Id, ProductId: ProductId, WarehouseId: WarehouseId,
                BaleQty: BaleQty, BaleWeight: BaleWeight,
                WeightInMon: WeightInMon, SupplierId: SupplierId, Product: Product,
                Supplier: Supplier, WareHouse: WareHouse
            };
            detailsStockOut.push(object);
            var templateWithData = Mustache.to_html($("#templateStockOutModal").html(), { StockTableAdd: detailsStockOut });
            $("#div-stockOut-add").empty().html(templateWithData);
        },
        error: function () {
            alert('An error occured try again later');
        }
    });
}