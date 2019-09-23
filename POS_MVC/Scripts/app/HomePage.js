function LoadStatistices() {
    var url = '/Sales/GetProductWiseSalesSummary';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            console.log(res);
            $("#totalBranch").text(res.TotalBranch);
            //$("#totalSales").text(res.TodaySales);
            //console.log(a);

            var templateWithData = Mustache.to_html($("#templateProductGroupModal").html(), { ProductGroupSearch: res});
            $("#div-product-add").empty().html(templateWithData);
            MakePagination('productGroupTableModal');
            console.log(res.TopSell);
        },
        error: function (err) {
            console.log(err);
        }
    });
}