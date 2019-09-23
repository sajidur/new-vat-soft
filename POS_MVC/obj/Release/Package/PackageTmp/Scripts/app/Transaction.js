
function GetAllLedgerPosting(fromDate,toDate) {
    $('#PaymentReportTableModal').DataTable({
        "pageLength": 100,
        "ajax": {
            "dataType": 'json',
            "contentType": "application/json; charset=utf-8",
            "type": "GET",
            "url": "/report/GetAllInvoice",
            "data": {'fromDate': fromDate, 'toDate': toDate },
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            { "data": "Value"},
            { "data": "LedgerName" },
            { "data": "VoucherNo" },
            { "data": "Extra1" },
            { "data": "Debit" },
            { "data": "Credit" }
        ],

        "columnDefs": [{
            "targets": 1,
            "data": "LedgerId",
            "render": function (data, type, row, meta) {
                return '<a  onclick="window.location.href=\'/Report/Viewer/ReportViewer.aspx?ReportName=LedgerReport&type=2&ledgerId=' + row.LedgerId + '\'">' + row.LedgerName + '</a>';
            }
        },{
            "targets": 0,
            "data": "Value",
            "render": function (data, type, row) {
                return moment(data).format("DD-MMM-YYYY");
            }
        }
        ],
        "bDestroy": true,
        "colReorder": true,
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
}


function GetReceipt(fromDate, toDate) {
    $('#PaymentReportTableModal').DataTable({
        "pageLength": 100,
        "ajax": {
            "dataType": 'json',
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "/Receipt/GetAllReceipt?fromDate=" + fromDate + "&toDate=" + toDate + "",
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            { "data": "Date" },
            { "data": "LedgerName" },
            { "data": "extra1" },
            { "data": "Debit" },
            { "data": "Credit" }
        ],
        "columnDefs": [{
            "targets": 1,
            "data": "LedgerId",
            "render": function (data, type, row, meta) {
                return '<a  onclick="window.location.href=\'/Report/Viewer/ReportViewer.aspx?ReportName=LedgerReport&type=1&ledgerId=' + row.LedgerId + '\'">' + row.LedgerName + '</a>';
            }
        }],
        "bDestroy": true,
        "dom": 'Bfrtip',
        "buttons": [
            {
                extend: "pdf",
                title: function () { return "M/s. Dada Auto Rice Mill\nHead Office: Khazanagor, Bisic, Kushtia, Bangladesh,\nE-mail:dadaricemill@gmail.com, Mobile: 01973-026852 \n Daily Receive Report"; },
                filename: function () { return "Daily Customer Transaction" + new Date().getDate()+"/"+new Date().getMonth()+1+"/"+new Date().getFullYear() }
            },
            {
                extend: "excel",
                title: function () { return "M/s. Dada Auto Rice Mill\nHead Office: Khazanagor, Bisic, Kushtia, Bangladesh, E-mail:dadaricemill@gmail.com, Mobile: 01973-026852 \n Daily Receive Report"; },
                filename: function () { return "Daily Customer Transaction" + new Date().getDate() + "/" + new Date().getMonth() + 1 + "/" + new Date().getFullYear() }

            }
        ]
    });
}
function GetPayment(fromDate,toDate) {
    $('#PaymentReportTableModal').DataTable({
        "pageLength": 100,
        "ajax": {
            "dataType": 'json',
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "/payment/GetAllInvoice?fromDate="+fromDate+"&toDate="+toDate+"",
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            { "data": "Date" },
            { "data": "LedgerName" },
            { "data": "extra1" },
            { "data": "Debit" },
            { "data": "Credit" }

        ],
        "columnDefs": [{
            "targets": 1,
            "data": "LedgerId",
            "render": function (data, type, row, meta) {
                return '<a  onclick="window.location.href=\'/Suppliers/SingleSupplierLedger?ledgerId=' + row.LedgerId + '&IsSupplier=0\'">' + row.LedgerName + '</a>';
            }
        }
        ],
        "bDestroy": true,
        "dom": 'Bfrtip',
        "buttons": [
                     {
                         extend: "pdf",
                         title: function () { return "M/s. Dada Auto Rice Mill\nHead Office: Khazanagor, Bisic, Kushtia, Bangladesh,\nE-mail:dadaricemill@gmail.com, Mobile: 01973-026852 \n Daily Payment Report"; },
                         filename: function () { return "Daily Payment Transaction" + new Date().getDate() + "/" + new Date().getMonth() + 1 + "/" + new Date().getFullYear() }
                     },
            {
                extend: "excel",
                title: function () { return "M/s. Dada Auto Rice Mill\nHead Office: Khazanagor, Bisic, Kushtia, Bangladesh, E-mail:dadaricemill@gmail.com, Mobile: 01973-026852 \n Daily Payment Report"; },
                filename: function () { return "Daily Payment Transaction" + new Date().getDate() + "/" + new Date().getMonth() + 1 + "/" + new Date().getFullYear() }

            }
        ]
    });
}

function DueSummary()
{
    $('#PaymentReportTableModal').DataTable({
        "pageLength": 100,
        "ajax": {
            "dataType": 'json',
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "/Customer/CustomerDueSummary",
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            { "data": "Name" },
            { "data": "Address" },
            { "data": "Phone" },
            { "data": "TotalInvoice" },
            { "data": "Ton" },
            { "data": "Debit" },
            { "data": "Credit" },
            { "data": "Balance" }

        ],
        "columnDefs": [{
            "targets": 0,
            "data": "LedgerId",
            "render": function (data, type, row, meta) {
                return '<a  onclick="window.location.href=\'/Customer/SingleCustomerLedger?ledgerId=' + row.LedgerId + '&IsSupplier=0\'">' + row.Name + '</a>';
            }
        }],
        "bDestroy": true,
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
}

function DueSummarySUpplier() {
    $('#PaymentReportTableModal').DataTable({
        "pageLength": 100,
        "ajax": {
            "dataType": 'json',
            "contentType": "application/json; charset=utf-8",
            "type": "POST",
            "url": "/Suppliers/SupplierDueSummary",
            "dataSrc": function (json) {
                return json;
            }
        },
        "columns": [
            { "data": "Name" },
            { "data": "Address" },
            { "data": "Phone" },
            { "data": "TotalInvoice" },
            { "data": "Ton" },
            { "data": "Debit" },
            { "data": "Credit" },
            { "data": "Balance" }
        ],
        "columnDefs": [{
            "targets": 0,
            "data": "LedgerId",
            "render": function (data, type, row, meta) {
                return '<a  onclick="window.location.href=\'/Suppliers/SingleSupplierLedger?ledgerId=' + row.LedgerId + '&IsSupplier=0\'">' + row.Name + '</a>';
            }
        }],
        "bDestroy": true,
        "dom": 'Bfrtip',
        "buttons": [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
}
