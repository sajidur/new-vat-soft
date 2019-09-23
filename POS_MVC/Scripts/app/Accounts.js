var ledgerPostingList = [];
function LoadForEdit(parameters) {
    $("#ui-id-1").html("Modify Account");
    $("#btnSave").hide();
    $("#btnUpdate").show();
    $("#btnDelete").show();
    LoadSingleData(parameters);
}

function LoadAccountGroupList() {
    var url = '/AccountGroup/GetAllAccountGroup';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var controlId = "ddlUnder";
            var data = res;
            //alert('Success');
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.AccountGroupName, item.Id);
                });
            }
            var templateWithData = Mustache.to_html($("#templateProductGroupModal").html(), { ProductGroupSearch: res });
            $("#div-productGroup").empty().html(templateWithData);
          //  MakePagination('productGroupTableModal');
        },
        error: function () {
        }
    });
}

function LoadAccountGroup() {
    var url = '/AccountGroup/GetAllAccountGroup';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var controlId = "ddlUnder";
            var data = res;
            //alert('Success');
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.AccountGroupName, item.Id);
                });
            }
        },
        error: function () {
        }
    });
}

function LoadAccountGroupListByNature(a) {
    console.log(a);
    var url = '/AccountGroup/GetAllGroupByNature';
    $.ajax({
        url: url,
        method: 'POST',
        data: { nature : a},
        success: function (res) {
            var controlId = "ddlUnder";
            var data = res;
            //alert('Success');
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.AccountGroupName, item.Id);
                });
            }
        },
        error: function () {
        }
    });
}
function LoadAccountLedgerList() {
    var url = '/AccountLedger/GetAllLedger';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var controlId = "ddlUnder";
            var data = res;
            //alert('Success');
          //  $("#" + controlId).empty();
           // $("#" + controlId).get(0).options.length = 0;
            //if (data != null) {
            //    $.each(data, function (index, item) {
            //        $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.AccountGroupName, item.Id);
            //    });
            //}
            var templateWithData = Mustache.to_html($("#templateProductGroupModal").html(), { ProductGroupSearch: res });
            $("#div-productGroup").empty().html(templateWithData);
            //  MakePagination('productGroupTableModal');
        },
        error: function () {
        }
    });
}
function LoadAccountLedger(controlId) {
    var url = '/AccountLedger/GetAllLedger';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var data = res;
            //alert('Success');
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.LedgerName, item.Id);
                });
            }
        },
        error: function () {
        }
    });
}

function GetBankCashLedgerList(controlId, groupId) {
    console.log(groupId);
    var url = '/AccountLedger/GetBankCashLedgerList';
    $.ajax({
        url: url,
        data: {'groupId': groupId },
        method: 'POST',
        success: function (res) {
            var data = res;
            //alert('Success');
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (data != null) {
                $.each(data, function (index, item) {
                    if (groupId == 28) {
                        $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.BranchName + "-" +item.BankAccountNumber + "-" + item.BranchCode, item.Id);
                    }
                    else {
                        $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.LedgerName, item.Id);
                    }
                    });
            }
        },
        error: function () {
        }
    });
}
function GetDrCrLedgerList(a, value) {
    var url = '/AccountLedger/DrCrLedgerList';
    console.log(value);
    $.ajax({
        url: url,
        method: 'POST',
        data: {DrCr: a},
        success: function (res) {
            var controlId = value;
            
            var data = res;
            //alert('Success');
            $("#" + controlId).empty();
            $("#" + controlId).get(0).options.length = 0;
            if (data != null) {
                $.each(data, function (index, item) {
                    $("#" + controlId).get(0).options[$("#" + controlId).get(0).options.length] = new Option(item.LedgerName, item.Id);
                });
            }
        },
        error: function () {
        }
    });
}
function LoadSingleData(parameters) {
    $.ajax({
        url: '/AccountLedger/GetById',
        data: { 'id': parameters },
        success: function (data) {
            $("#hdId").val(data.Id);
            $("#txtLedgerName").val(data.LedgerName);
            $("#ddlNature").val(data.CrOrDr);
        },
        error: function () {
            alert('An error occured try again later');
        }
    });
}

function ResetForm() {
    $('#txtCustomerName').val('');
    $('#txtCPName').val('');
    $('#txtAddress').val('');
    $('#txtPhone').val('');
    $('#txtEmail').val('');
    $('#txtDescription').val('');
    $('#txtAccountNumber').val(''),
    $('#txtBankName').val(''),
    $('#txtBranchName').val(''),
    $("#btnSave").show();
}

function SaveAccounts() {
    $.ajax({
        url: '/AccountGroup/Create',
        method: 'post',
        dataType: 'json',
        async: false,
        data: {
            AccountGroupName: $('#txtGroupName').val(),
            GroupUnder: $('#ddlUnder').val(),
            Narration: $('#txtDescription').val(),
            Nature: $('#ddlNature').val(),
            create: 1
        },
        success: function (data) {
            setTimeout(location.reload.bind(location), 10000);
            ShowNotification("1", "Account Saved Saved!!");
            ResetForm();
            LoadCustomerList();
        },
        error: function () {

        }
    });

}


function SaveLedger() {
    $.ajax({
        url: '/AccountLedger/Create',
        method: 'post',
        dataType: 'json',
        async: false,
        data: {
            LedgerName: $('#txtLedgerName').val(),
            AccountGroupId: $('#ddlUnder').val(),
            OpeningBalance: $('#txtOpeningBalance').val(),
            Narration: $('#txtDescription').val(),
            CrOrDr: $('#ddlNature').val(),
            BankAccountNumber: $('#txtAccountNumber').val(),
            BranchName: $('#txtBankName').val(),
            BranchCode: $('#txtBranchName').val(),
            create: 1
        },
        success: function (data) {
            setTimeout(location.reload.bind(location), 10000);
            ShowNotification("1", "Ledger Saved!!");
            ResetForm();
            LoadAccountLedgerList();
        },
        error: function (err) {
            console.log(err);

        }
    });

}

$("#btnUpdate").click(function () {
    var formObject = FormDataAsObject();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '/Customer/Edit',
        data: {
            Id: $("#hdId").val(),
            Name: formObject.Name,
            ContactPersonName: formObject.ContactPersonName,
            Address: formObject.Address,
            Phone: formObject.Phone,
            Email: formObject.Email,
            Description: formObject.Description,
            create: 1
        },
        async: false,
        success: function (data) {
            if (data.result == false) {
                $("#lblMessage").html(data.Error);
            }
            setTimeout(location.reload.bind(location), 10000);
            //alert('Update Successfully.');
            ShowNotification("1", "Customer Updated!!");
            $("#btnSave").show();
            LoadCustomerList();
            ResetForm();

        },
        error: function () {
            alert('An error occured try again later');
        }
    });
});

$("#btnDelete").click(function () {
    $.ajax({
        type: 'POST',
        url: '/Accountledger/Delete',
        dataType: 'json',
        data: {
            Id: $("#hdId").val(),
            create: 1
        },
        async: false,
        success: function (data) {
            setTimeout(location.reload.bind(location), 10000);
            //alert('Delete Successfully.');
            ShowNotification(data.id, data.message);
            $("#btnSave").show();
            ResetForm();
        },
        error: function () {
            alert('An error occured try again later');
        }
    });
});

function ChangeGroup()
{
    var id = $("#ddlUnder").val();
    $.ajax({
        url: '/AccountGroup/GetById',
        data: { 'groupId': id },
        success: function (data) {
            if (data.GroupUnder==-1) {
                $('#ddlNature').attr("disabled", false);
                $('#chkAffectGrossProfit').attr("disabled", false);
            }
            else
            {
                $('#ddlNature').attr("disabled", true);
                $('#ddlNature').val(data.Nature);
                $('#chkAffectGrossProfit').attr("disabled", true);
            }
            if (data.AffectGrossProfit==0) {
                $('#chkAffectGrossProfit').prop('checked', false);
            }
            else {
                $('#chkAffectGrossProfit').prop('checked', true);
            }
            $("#hdId").val(data.Id);
        },
        error: function () {
            alert('An error occured try again later');
        }
    });

}
function ChangeLedgerGroup() {
    var id = $("#ddlUnder").val();
    if (id == 28) {
        console.log(id + "selected val");

        $("#divBank").css("display", "block");
    }
    else {
        $("#divBank").css("display", "none");
    }
    $.ajax({
        url: '/AccountLedger/GetLedgerNature',
        data: { 'groupId': id },
        success: function (data) {

                $('#ddlNature').val(data);
        },
        error: function () {
            alert('An error occured try again later');
        }
    });

}

function JournalPosting() {
    $.ajax({
        url: '/AccountGroup/Create',
        method: 'post',
        dataType: 'json',
        async: false,
        data: {
            AccountGroupName: $('#txtGroupName').val(),
            GroupUnder: $('#ddlUnder').val(),
            Narration: $('#txtDescription').val(),
            Nature: $('#ddlNature').val(),
            create: 1
        },
        success: function (data) {
            setTimeout(location.reload.bind(location), 10000);
            ShowNotification("1", "Account Saved Saved!!");
            ResetForm();
            LoadCustomerList();
        },
        error: function () {
        }
    });
}

var count = 0;
function AddJournalToGrid() {
    var Ledger = "";
    var DrOrCr = '';
    var Amount = '';
    var ChequeNo = '';
    var ChequeDate = '';
    $('#myTable tr').each(function (i) {
        if (i == "1") {
            var countCount = count++;
            LedgerId = $("#ddlLedger option:selected").val();
            LedgerName = $("#ddlLedger option:selected").text();
            DrOrCr = $(this).find('td').eq(1).find('select').val();
            Amount = $(this).find('td').eq(3).find('input').val();
            ChequeNo = $(this).find('td').eq(4).find('input').val();
            ChequeDate = $(this).find('td').eq(5).find('input').val();
        }

    });
    var object = {
        Id: count,
        LedgerId: LedgerId,
        LedgerName:LedgerName,
        DrOrCr: DrOrCr,
        Amount: Amount,
        ChequeNo: ChequeNo,
        ChequeDate: ChequeDate
    };
    ledgerPostingList.push(object);
    var templateWithData = Mustache.to_html($("#templateProductModalAdd").html(), { ProductSearchAdd: ledgerPostingList });
    $("#div-product-add").empty().html(templateWithData);
}
