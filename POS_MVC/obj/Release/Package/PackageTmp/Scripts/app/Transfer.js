$("#btnFilter").click(function () {
    try {
        var fromDate = $('#txtFromDate').val();
        var toDate = $('#txtToDate').val();
        var AccountLedgerId = $("#ddlSupplier option:selected").val();
        var url = '/LedgerPosting/GetAllLedgerPosting';
        $.ajax({
            url: url,
            method: 'POST',
            data: { 'fromDate': fromDate, 'toDate': toDate, 'id': AccountLedgerId,'VoucherTypeId':5 },
            success: function (res) {
                var templateWithData = Mustache.to_html($("#templateProductGroupModal").html(), { ProductGroupSearch: res });
                $("#div-LedgerPosting").empty().html(templateWithData);
                //MakePagination('salesOrderGroupTableModal');
            },
            error: function (error, r) {
                ShowNotification("3", "Something Wrong!!");
            }
        });
    }catch(ex)
    {
        console.log(ex);
    }
});