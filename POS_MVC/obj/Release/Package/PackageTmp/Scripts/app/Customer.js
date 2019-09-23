function LoadListData() {
    var url = '/Customer/GetAll';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var templateWithData = Mustache.to_html($("#templateProductGroupModal").html(), { ProductGroupSearch: res });
            $("#div-productGroup").empty().html(templateWithData);
            MakePagination('productGroupTableModal');
        },
        error: function () {
        }
    });
}

function LoadForEdit(parameters) {
    $("#ui-id-1").html("Modify Category");
    $("#btnSave").hide();
    $("#btnUpdate").show();
    $("#btnDelete").show();

    LoadSingleData(parameters);
}

function LoadCustomerList(controlId) {
    var url = '/Customer/GetAll';
    $.ajax({
        url: url,
        method: 'POST',
        success: function (res) {
            var data = res;
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
            $("#" + controlId).chosen({ no_results_text: "Oops, nothing found!" });
            var templateWithData = Mustache.to_html($("#templateProductGroupModal").html(), { ProductGroupSearch: res });
            $("#div-productGroup").empty().html(templateWithData);
            MakePagination('productGroupTableModal');
        },
        error: function () {
        }
    });
}

function LoadSingleData(parameters) {
    $.ajax({
        url: '/Customer/Details',
        data: { 'id': parameters },
        success: function (data) {
            $("#hdId").val(data.Id);
            $("#txtCustomerName").val(data.Name);
            $("#txtCustomerCode").val(data.Code);
            $("#txtCPName").val(data.ContactPersonName);
            $("#txtAddress").val(data.Address);
            $("#txtPhone").val(data.Phone);
            $("#txtEmail").val(data.Email);
            $("#txtLimit").val(data.Limit);
            $("#txtDescription").val(data.Description);
        },
        error: function () {
            alert('An error occured try again later');
        }
    });
}

function FormDataAsObject() {
	var object = new Object();
	object.Name = $('#txtCustomerName').val();
	object.Code = $('#txtCustomerCode').val();
	object.ContactPersonName = $('#txtCPName').val();
	object.Address = $('#txtAddress').val();
	object.Phone = $('#txtPhone').val();
	object.Email = $('#txtEmail').val();
	object.Limit = $('#txtLimit').val();
	object.Description = $('#txtDescription').val();
	
	return object;
}

function ResetForm() {
    $('#txtCustomerName').val('');
    $('#txtCPName').val('');
    $('#txtCustomerCode').val('');
    $('#txtAddress').val('');
    $('#txtPhone').val('');
    $('#txtEmail').val('');
    $('#txtDescription').val('');
    $('#txtLimit').val('');
    $("#btnSave").show();
}

function Save() {

    if ($("#txtCustomerName").val() == "") {
        alert('Please give customer name.');
        return false;
    }
	var formObject = FormDataAsObject();
	$.ajax({
		url: '/Customer/Create',
		method: 'post',
		dataType: 'json',
		async: false,
		data: {
		    Name: formObject.Name,
            Code:formObject.Code,
			ContactPersonName: formObject.ContactPersonName,
			Address: formObject.Address,
			Phone: formObject.Phone,
			Email: formObject.Email,
			Description: formObject.Description,
			OpeningBalance: $('#txtOpeningBalance').val(),
			CrOrDr: $('#ddlNature').val(),
            Limit:formObject.Limit,
			create: 1
		},
		success: function (data) {
		    ShowNotification("1", "Customer Saved!!");
		    ResetForm();
		    LoadListData();
		    LoadCustomerList();
		},
		error: function () {

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
            Code: formObject.Code,
            ContactPersonName: formObject.ContactPersonName,
            Address: formObject.Address,
            Phone: formObject.Phone,
            Email: formObject.Email,
            Limit:formObject.Limit,
            Description: formObject.Description,
            OpeningBalance: $('#txtOpeningBalance').val(),
            CrOrDr: $('#ddlNature').val(),
            create: 1
        },
        async: false,
        success: function (data) {
            if (data.result == false) {
                $("#lblMessage").html(data.Error);
            }
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
        url: '/Customer/Delete',
        dataType: 'json',
        data: {
            Id: $("#hdId").val(),
            create: 1
        },
        async: false,
        success: function (data) {
            if (data.result == false) {
                $("#lblMessage").html(data.Error);
            }
            //alert('Delete Successfully.');
            ShowNotification("1", "Customer Deleted!!");
            $("#btnSave").show();
            LoadCustomerList();
            ResetForm();
        },
        error: function () {
            alert('An error occured try again later');
        }
    });
});