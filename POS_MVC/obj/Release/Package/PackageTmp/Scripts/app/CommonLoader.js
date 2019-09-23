function LoadAllWareHouse(controlId) {
    var url = "/WareHouse/GetAll";
    $.ajax({
        url: url,
        method: "POST",
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
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.WareHouseName, item.Id);
                });
            }
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!" });

        },
        error: function () {
        }
    });
}
function LoadUnit(controlId) {
    var url = "/Units/GetAll";
    $.ajax({
        url: url,
        method: "POST",
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
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.Name, item.Id);
                });
            }
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!" });

        },
        error: function () {
        }
    });
}
function LoadSupplierCombo(controlId) {
    var url = '/Suppliers/GetAll';

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
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(''+item.Code+'-'+item.Name, item.Id);
                });
            }
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!", search_contains: true });
        },
        error: function () {
        }
    });
}

function LoadSupplierComboByLedgerId(controlId) {
    var url = '/Suppliers/GetAll';

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
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option('' + item.Code + '-' + item.Name, item.LedgerId);
                });
            }
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!", search_contains: true });
        },
        error: function () {
        }
    });
}
function LoadAllBranch(controlId) {
    var url = "/Branch/GetAll";
    $.ajax({
        url: url,
        method: "POST",
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
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.Name, item.Id);
                });
            }
        },
        error: function () {
        }
    });
}
function LoadAllProduct(controlId) {
    var url = "/Product/GetAll";
    $.ajax({
        url: url,
        method: "POST",
        success: function (res) {
            var data = res;
            //alert('Success');
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (true) {
                $("#" + controlId).get(0).options[0] = new Option("-Select-", "-1");
            }
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.ProductName, item.Id);
                });
            }
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!" });
        },
        error: function (err) {
            console.log(err);
        }
    });
}
function LoadCustomerCombo(controlId) {
    var url = '/Customer/GetAll';

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

function LoadCustomerComboByLedgerId(controlId) {
    var url = '/Customer/GetAll';

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
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option('' + item.Code + '-' + item.Name, item.LedgerId);
                });
            }
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!", search_contains: true });
        },
        error: function () {
        }
    });
}

function LoadAllProduct(type, controlId) {
    var url = "/Product/GetAll?Type=" + type + "";
    $.ajax({
        url: url,
        method: "POST",
        success: function (res) {
            var data = res;
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (true) {
                $("#" + controlId).get(0).options[0] = new Option("-Select-", "-1");
            }
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.Code + "-" + item.ProductName, item.Id);
                });
            }
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!" });

        },
        error: function () {
        }
    });

}

function dateConvert(dateObject) {
    var d = new Date(dateObject);
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var date =year+"-"+month+"-"+day+"";

    return date;
}
