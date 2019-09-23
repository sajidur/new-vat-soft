var paymentList = [];
var aabb = [];
var details = [];

function LoadAllLocalPayment() {
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    console.log(fromDate);
    console.log(toDate);
    var url = '/Payment/LoadAllLocalPayment?fromDate='+fromDate+'&toDate='+toDate;
    $.ajax({
        url: url,
        method: 'GET',
        success: function (res) {
            aabb.push(res);
            console.log(aabb);
            var templateWithData = Mustache.to_html($("#templateProductGroupModal").html(), { ProductGroupSearch: res });
            $("#div-productGroup").empty().html(templateWithData);
            // MakePagination('productGroupTableModal');
        },
        error: function (err) {
            console.log(err);
        }
    });
}

var count = 1;
function LoadForAddOrder(InvId) {
    var countCount = count++;
    var Party = "";
    var date="";
    var CashAmount = "";
    var ChequeAmount = "";
    var LedgerID = "";
    var CreditAmount= "";
    $('#productGroupTableModal tr').each(function (i) {
        var invoice= $(this).find('td').eq(2).text();
        if (invoice == InvId) {
            date = $(this).find('td').eq(0).html();
            Party = $(this).find('td').eq(1).text();
            CashAmount = parseFloat($(this).find('td').eq(6).find('input').val());
            ChequeAmount = parseFloat($(this).find('td').eq(7).find('input').val());
            LedgerID = parseFloat($(this).find('td').eq(9).text());
            CreditAmount = parseFloat($(this).find('td').eq(3).text());
        }
    });
    var object = {
        countCount: countCount,
        PostingDate: date,
        InvoiceNo: InvId,
        Party: Party,
        Debit: CashAmount,
        Credit: ChequeAmount,
        LedgerId: LedgerID,
        Balance: CreditAmount
    };
    paymentList.push(object);
    console.log(paymentList);
    
    var templateWithData = Mustache.to_html($("#templateProductGroupModal_added").html(), { ProductGroupSearch_added: paymentList });
    $("#div-productGroup-added").empty().html(templateWithData);
    Calculation();
}

function Delete(Id) {
    for (var i = 0; i < paymentList.length; i++) {
        if (paymentList[i].countCount == Id) {
            paymentList.splice(i, 1);
        }
    }
    
    var templateWithData = Mustache.to_html($("#templateProductGroupModal_added").html(), { ProductGroupSearch_added: paymentList });
    $("#div-productGroup-added").empty().html(templateWithData);
    Calculation();
}

$("#btnAddPayment").click(function () {
    var AccountGroup = $("#ddlBankCash option:selected").text();
    var AccountGroupId = $("ddlBankCash option:selected").val();
    var AccountLedger = $("#ddlItem option:selected").text();
    var AccountLedgerId = $("#ddlItem option:selected").val();
    var chkno = $("#txtChequeNo").val();
    var PostingDate = $("#txtChequeDate").val();
    var CasAmount = parseFloat($("#lblTotalCashAmount").text());
    var Chequeamount = parseFloat($("#lblTotalChequeAmount").text());
    var amount=0;
    if (AccountGroup == "Cash") {
        amount = parseFloat($("#lblTotalCashAmount").text());
    } else { amount = parseFloat(Chequeamount); }
    if (PostingDate == "0")
    {
        ShowNotification("3", "Choose Date!!");
        return;
    }
    if (AccountLedgerId <= 0) {
        ShowNotification("3", "Choose any Ledger!!");
        return;
    }
    //var object = {Id:Id, Item: item, BaleQty: BaleQty, QtyPerBale: QtyPerBale, UnitStyle: UnitStyle, Amount: Amount, TotalKg: TotalKg, TotalQty: TotalQty, TotalAmount: TotalAmount };
    var object = { count: count, GroupId: AccountGroupId, GroupName: AccountGroup, LedgerId: AccountLedgerId, LedgerName: AccountLedger, ChequeNo: chkno, ChequeDate: PostingDate, Credit: amount };
    console.log(object);
    details.push(object);
    var templateWithData = Mustache.to_html($("#templatePaymentModal").html(), { PaymentSearch: details });
    $("#div-Payment-added").empty().html(templateWithData);
    //CalculateSum();
    //DrCrCalculation();
    $("#chkNo").val("");
    $("#chkDate").val("");
    $("#txtAmount").text("");
    count++;
});

function PaymentDelete(Id) {
    for (var i = 0; i < details.length; i++) {
        if (details[i].count == Id) {
            details.splice(i, 1);
        }
    }
}

function Save() {
    var url = '/Payment/LocalPaymentSave';
    var Tamount = parseFloat($("#lblTotalCashAchequeAmount").text());
    $.ajax({
        url: url,
        method: 'POST',
        data: {
            TotalAmount: Tamount,
            partyBalanceList: paymentList,
            ledgerPostingList: details
           
        },
        success: function (data) {
            ShowNotification("1", "Sales Saved!!");

            detailsSales = [];
            var templateWithData = Mustache.to_html($("#templateSalesGroupModalGrid").html(), { SalesGroupSearchGrid: detailsSales });
            $("#div-sales-add").empty().html(templateWithData);
            var invoice = $("#txtPoNo").val();
            window.open("/Report/Viewer/ReportViewer.aspx?ReportName=SalesInvoice&invoiceId=" + invoice, "_blank");
            // window.location.href = ;
        },
        error: function (err) {
            console.log(err);
        }
    });
}


function Calculation()
{
    var TotalCash = 0.0;
    var TotalCheque = 0.0;
    var Total = 0.0;
    try {

        for (var i = 0; i < paymentList.length; i++) {
            console.log(paymentList[i].Debit);
            TotalCash += parseFloat(paymentList[i].Debit);
            TotalCheque += parseFloat(paymentList[i].Credit);
        }
        console.log(TotalCash);
        console.log(TotalCheque);
        
        Total = TotalCash + TotalCheque;
        console.log(Total);
        $("#lblTotalCashAmount").html(TotalCash.toFixed(2));
        $("#lblTotalChequeAmount").html(TotalCheque.toFixed(2));
        $("#lblTotalCashAchequeAmount").html(Total.toFixed(2));

    } catch (e) {
        console.log(e);

    }
}