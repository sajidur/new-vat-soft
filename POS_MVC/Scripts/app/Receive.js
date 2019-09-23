function LoadForReeive() {
    var fromDate = $('#txtToDate').val();
    var toDate = $('#txtToDate').val();
    console.log(fromDate);
    var url = '/Payment/LoadAllLocalPayment?fromDate=' + fromDate + '&toDate=' + toDate;
    $.ajax({
        url: url,
        method: 'GET',
        success: function (res) {
            var templateWithData = Mustache.to_html($("#templateProductGroupModal").html(), { ProductGroupSearch: res });
            $("#div-productGroup").empty().html(templateWithData);
            MakePagination('productGroupTableModal');
        },
        error: function (err) {
            console.log(err);
        }
    });
}
