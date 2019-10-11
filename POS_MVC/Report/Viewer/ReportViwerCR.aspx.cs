using CrystalDecisions.CrystalReports.Engine;
using REX_MVC.BAL;
using REX_MVC.Report.Viewer;
using REX_MVC.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace REX_MVC.Report.Viewer
{
    public partial class ReportViwerCR : System.Web.UI.Page
    {
        Result oResult = new Result();
        SQLDAL oDAL = new SQLDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int yearId = CurrentSession.GetCurrentSession().FinancialYear;
                try
                {
                    string reportType = Request.QueryString["ReportName"].ToString();
                    if (reportType == "Mushak11")
                    {
                        Mushak11();
                    }
                    if (reportType == "EXECLIPARTReport")
                    {
                        // LoadEXECLIPARTReport();
                    }
                    if (reportType == "SalesInvoice")
                    {
                        Mushak11();
                    }
                    if (reportType == "StockOutForProcessing")
                    {
                        StockOutForProcessing();
                    }
                    if (reportType == "CustomerDue")
                    {
                        LoadCustomerDueReport();
                    }
                    if (reportType == "SalesDescriptions")
                    {
                        LoadSalesDescriptions();
                    }
                    if (reportType == "SuplierTransaction")
                    {
                        SuplierTransaction(yearId);
                    }
                    if (reportType == "CustomerTransaction")
                    {
                        CustomerTransaction();
                    }
                    if (reportType == "IncomeStatement")
                    {
                        IncomeStatment();

                    }
                    if (reportType == "LedgerReport")
                    {
                        LedgerReport();
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.InnerHtml = ex.Message;
                }
            }
        }

        private void Mushak11()
        {
            string query = @"exec rptSalesInvoice";
           // oResult = oDAL.Select(query);
            DataTable dt = null;
            dt = oResult.Data as DataTable;

            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(Server.MapPath("/Report/RPT/Mushak11/rptMushak11_v3.rpt"));
            cryRpt.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = cryRpt;


        }

        private void IncomeStatment()
        {
            string fromDate = Request.QueryString["fromDate"].ToString();
            string toDate = Request.QueryString["toDate"].ToString();
            string query = @"exec IncomeStatement '" + fromDate + "','" + toDate + "'";
            oResult = oDAL.Select(query);
            DataSet dt = null;
            dt = oResult.ds as DataSet;
          
        }
        private void StockOutForProcessing()
        {
            string invoiceId = Request.QueryString["invoiceId"].ToString();
            string query = @"exec StockOutInvoice '" + invoiceId + "'";
            oResult = oDAL.Select(query);
            DataTable dt = null;
            dt = oResult.Data as DataTable;
          
        }

        private void LoadCustomerDueReport()
        {
            //string invoiceId = Request.QueryString["invoiceId"].ToString();
            string query = @"exec CustomerDueSummary";
            oResult = oDAL.Select(query);
            DataTable dt = null;
            dt = oResult.Data as DataTable;
         
        }

        private void LoadSalesDescriptions()
        {
            string invoiceId = Request.QueryString["invoiceId"].ToString();
            string query = @"exec rptSalesDescription '" + invoiceId + "'"; //
            oResult = oDAL.Select(query);
            DataTable dt = null;
            dt = oResult.Data as DataTable;


            query = @"exec rptSalesDueDescription '" + invoiceId + "'";   //'" + customerId + "'
            oResult = oDAL.Select(query);
            dt = oResult.Data as DataTable;
        

            query = @"exec rptSalesCreditDescription '" + invoiceId + "'";   //'" + customerId + "'
            oResult = oDAL.Select(query);
            dt = oResult.Data as DataTable;
           
        }

        private void SuplierTransaction(int yearId)
        {
            string type = Request.QueryString["type"].ToString();
            string query = @"exec DueSummary '1'," + type + "," + yearId + "";
            oResult = oDAL.Select(query);
            DataSet dt = null;
            dt = oResult.ds as DataSet;
         
        }

        private void LedgerReport()
        {
            string type = Request.QueryString["type"].ToString();
            string ledgerId = Request.QueryString["ledgerId"].ToString();
            string isSupplier = Request.QueryString["IsSupplier"];
            string fromDate = Request.QueryString["fromDate"].ToString();
            string toDate = Request.QueryString["toDate"].ToString();
            var yearId = CurrentSession.GetCurrentSession().FinancialYear;
            string query = @"exec rptCustomerLedger '" + ledgerId + "'," + type + "," + yearId + ",'" + fromDate + "','" + toDate + "'";
            oResult = oDAL.Select(query);
            DataSet dt = null;
            dt = oResult.ds as DataSet;
         


            query = @"exec rptIndividualLedger " + ledgerId + ",1," + isSupplier + "";
            oResult = oDAL.Select(query);
            dt = oResult.ds as DataSet;
         
        }

        private void CustomerTransaction()
        {
            string invoiceId = Request.QueryString["invoiceId"].ToString();
            string query = @"exec rptCustomerTransaction '" + invoiceId + "'";
            oResult = oDAL.Select(query);
            DataTable dt = null;
            dt = oResult.Data as DataTable;
        
        }

        protected void btnTruckChallan_Click(object sender, EventArgs e)
        {
            //btnTruckChallan.Visible = true;
            //string invoiceId = Request.QueryString["invoiceId"].ToString();
            //string query = @"exec rptSalesInvoice '" + invoiceId + "'";
            //oResult = oDAL.Select(query);
            //DataTable dt = null;
            //dt = oResult.Data as DataTable;

            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/RPT/rptTruckChallanInvoice.rdlc");
            //ReportDataSource datasource = new ReportDataSource("dsSalesInvoice", dt);
            //ReportViewer1.LocalReport.DataSources.Clear();
            //ReportViewer1.LocalReport.DataSources.Add(datasource);

            //string customerId = Request.QueryString["customerid"].ToString();
            //query = @"exec rptIndividualLedger " + customerId + "";
            //oResult = oDAL.Select(query);
            //dt = oResult.Data as DataTable;
            //ReportDataSource customerDatasource = new ReportDataSource("individualLedger", dt);
            //ReportViewer1.LocalReport.DataSources.Add(customerDatasource);

            //query = @"exec rptCustomerRecive " + customerId + "";
            //oResult = oDAL.Select(query);
            //dt = oResult.Data as DataTable;
            //ReportDataSource transactionDatasource = new ReportDataSource("rptCustomerTransaction", dt);
            //ReportViewer1.LocalReport.DataSources.Add(transactionDatasource);


        }

    }
}