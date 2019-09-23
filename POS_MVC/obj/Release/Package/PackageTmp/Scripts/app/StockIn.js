var detailsStockIn = [];
var datatableRowCount = 0;

$("#btnAdd").click(function () {
    try {
        datatableRowCount++;
    var item = $("#ddlItem option:selected").text();
    var ProductId = $("#ddlItem option:selected").val();
    var WarehouseId = $("#ddlWareHouse option:selected").val();
    var WarehouseName = $("#ddlWareHouse option:selected").text();
    var BaleQty = $("#txtBaleQty").val();
    var BaleWeight = $("#txtBaleWeight").val();
    var WarehouseId = $("#ddlWareHouse option:selected").val();
    var WeightInMon = $("#txtWeightInMon").val();
    var object = {
        Item: item, ProductId: ProductId, WarehouseId: WarehouseId,
        WarehouseName: WarehouseName, BaleQty: BaleQty, BaleWeight: BaleWeight,
        WeightInMon: WeightInMon
    };
    detailsStockIn.push(object);
    var templateWithData = Mustache.to_html($("#templateProductModalAdded").html(), { ProductSearchAdded: detailsStockIn });
    $("#div-product-added").empty().html(templateWithData);

    } catch (e) {
        console.log(e);

    }
});

$("#txtBaleQty").on("propertychange change keyup paste input", function () {
    // do stuff;
    Calculation();
});
$("#txtBaleWeight").on("propertychange change keyup paste input", function () {
    // do stuff;
    Calculation();
});
function Calculation() {
    var BaleQty = $("#txtBaleQty").val();
    var QtyPerBale = $("#txtBaleWeight").val();
    if (BaleQty == '') {
        BaleQty = 0;
    }
    if (QtyPerBale == '') {
        QtyPerBale = 0;
    }
    var totalInKG = QtyPerBale * BaleQty;
    $("#txtWeightInMon").val(totalInKG);
}

function OnDeleteProduct(Id) {
    detailsStockIn.splice(Id, 1);
    datatableRowCount--;
    var templateWithData = Mustache.to_html($("#templateProductModalAdded").html(), { ProductSearchAdded: detailsStockIn });
    $("#div-product-added").empty().html(templateWithData);
}

function LoadAllStockOutChallan() {
    var url = "/ProductionProcessing/GetAllStockOutChallanList";
    $.ajax({
        url: url,
        method: "POST",
        success: function (res) {
            var data = res;
            var controlId = "ddlStockOutInvoice";
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (true) {
                $("#" + controlId).get(0).options[0] = new Option("-পছন্দ করুন-", "-1");
            }
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item, item);
                });
            }
        },
        error: function () {
        }
    });

}


function LoadInvoiceNo(controlId) {
    var url = "/ProductionProcessing/GetInvoiceNumberForStockIn";
    $.ajax({
        url: url,
        method: "POST",
        success: function (res) {
            var data = res;
            $("#" + controlId).val(data);
        },
        error: function () {
        }
    });
}


function Save() {
    GetDataFromDatatable();
    var url = '/ProductionProcessing/SaveStockIn';
    $.ajax({
        url: url,
        method: 'POST',
        data: {
            InvoiceNo: $("#txtPoNo").val(),
            //SupplierId: $("#ddlSupplier option:selected").val(),
            Notes: $("#txtDescriptions").val(),
            stockIns: detailsStockIn
        },
        success: function (data) {
            ShowNotification("1", "Stock In Saved!!");
            $('#StockTableAdd').val("");
            setTimeout(location.reload.bind(location), 10000);
        },
        error: function (err) {
            ShowNotification("4", "ERROR!!"+err);

        }
    });

}

function GetDataFromDatatable() {
    $('#inventoryGroupTableModal tr').each(function(i) {
        if (i > 0) {
            var ProductId = $(this).find('td').eq(0).text();
            var BaleQty = $(this).find('td').eq(2).find('input').val();
            var BaleWeight = $(this).find('td').eq(3).find('input').val();
            var WeightInMon = $(this).find('td').eq(4).find('input').val();
            var WarehouseId = $(this).find('td').eq(5).find('select').val();
            var SupplierId = $("#ddlSupplier option:selected").val();
            //var SupplierId = GetSupplierId(ProductId);

            var object = {
                ProductId: ProductId,
                WarehouseId: WarehouseId,
                BaleQty: BaleQty,
                BaleWeight: BaleWeight,
                WeightInMon: WeightInMon,
                SupplierId: SupplierId
            };

            detailsStockIn.push(object);

        }
    });
}

function GetSupplierId(parameters) {
    var supplierId = '';
    $.ajax({
        url: '/Inventory/GetSupplierId',
        data: { 'id': parameters },
        success: function(data) {
            supplierId = data.SupplierId;
        }
    });
    return supplierId;
}