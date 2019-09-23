﻿
$(document).ready(function () {
    GetDataTable();

});

function GetDataTable() {
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
            "url": "/GoodsReceive/GetAllPaddyReceives",
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            {
                "data": "CreatedDate",
                'render': function (jsonDate) {
                    var date = new Date(parseInt(jsonDate.substr(6)));
                    var month = date.getMonth() + 1;
                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            { "data": "InvoiceNo" },
            { "data": "InvoiceNoPaper" },
            { "data": "Supplier.Name" },
            { "data": "AdditionalCost" },
            { "data": "TotalAmount" },
            { "data": "GrandTotal" },
            { "data": "Notes" }

        ],

        "bDestroy": true,
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
}

function GetDataForCustomer() {
    if ($("#ddlSupplier option:selected").val() == '') {
        GetDataTable();
    }
    else {
        GetDataTableForCustomer();
    }

}

function GetDataTableForCustomer() {
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
            "url": "/GoodsReceive/GetAllPaddyReceivesFilteredBySupplier?supplierId=" + $("#ddlSupplier option:selected").val(),
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            {
                "data": "CreatedDate",
                'render': function (jsonDate) {
                    var date = new Date(parseInt(jsonDate.substr(6)));
                    var month = date.getMonth() + 1;
                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            {"data":"InvoiceNo"},
            { "data": "InvoiceNoPaper" },
            { "data": "Supplier.Name" },
            { "data": "AdditionalCost" },
            { "data": "TotalAmount" },
            { "data": "GrandTotal" },
            { "data": "Notes" }

        ],
        "bDestroy": true,
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
}

$("#btnAddSalesReport").click(function (e) {
    if ($("#dateFromDate").val() == "") {
        alert('From Date Is Empty.');
        return false;
    }

    if ($("#dateToDate").val() == "") {
        alert('To Date Is Empty.');
        return false;
    }
    if ($("#ddlSupplier option:selected").val() == '') {
        GetDataTableForDate();
    }

    if ($("#ddlSupplier option:selected").val() != '') {
        GetDataTableForDateAndCustomer();
    }

});

function GetDataTableForDate() {
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
            "url": "/GoodsReceive/GetAllPaddyReceivesFilteredByDate?fromDate=" + $("#dateFromDate").val() + "&toDate=" + $("#dateToDate").val(),
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            {
                "data": "CreatedDate",
                'render': function (jsonDate) {
                    var date = new Date(parseInt(jsonDate.substr(6)));
                    var month = date.getMonth() + 1;
                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            { "data": "InvoiceNo" },
            { "data": "InvoiceNoPaper" },
            { "data": "Supplier.Name" },
            { "data": "AdditionalCost" },
            { "data": "TotalAmount" },
            { "data": "GrandTotal" },
            { "data": "Notes" }

        ],
        "bDestroy": true,
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
}

function GetDataTableForDateAndCustomer() {
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
            "url": "/GoodsReceive/GetAllPaddyReceivesFilteredBySupplierAndDate?fromDate=" + $("#dateFromDate").val() + "&toDate=" + $("#dateToDate").val() + "&supplierId=" + $('#ddlSupplier').val(),
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            {
                "data": "CreatedDate",
                'render': function (jsonDate) {
                    var date = new Date(parseInt(jsonDate.substr(6)));
                    var month = date.getMonth() + 1;
                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            { "data": "InvoiceNo" },
            { "data": "InvoiceNoPaper" },
            { "data": "Supplier.Name" },
            { "data": "AdditionalCost" },
            { "data": "TotalAmount" },
            { "data": "GrandTotal" },
            { "data": "Notes" }

        ],
        "bDestroy": true,
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
}

