<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViwerCR.aspx.cs" Inherits="REX_MVC.Report.Viewer.ReportViwerCR" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <label runat="server" id="lblMsg"></label>
    <div>
    
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    
    </div>
    </form>
</body>
</html>
