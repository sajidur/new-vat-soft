function LoadReceivePayment(supplierId) {
    var url = '/Payment/LoadAllLocalPayment?supplierId=' + supplierId;
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
