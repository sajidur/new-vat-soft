using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RiceMill_MVC.ServiceReference1;
using RestSharp;
using System.Configuration;

namespace RiceMill_MVC.BAL
{
    public class SMSEmailService
    {
        private string userName = "01719304970";

        private string userPassword = "bl01917813583";

        public SMSEmailService()
        {
        }

        public string SendOneToManyBulkSms(string mobileNumber, string smsText, string campaign)
        {
            string many;
            try
            {
                SendSmsSoapClient sms = new SendSmsSoapClient();
                many = sms.OneToMany(this.userName, this.userPassword, smsText, mobileNumber, "TEXT", "", campaign);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                ex.ErrorWritter();
                throw ex;
            }
            return many;
        }

        public string SendOneToOneSingleSms(string mobileNumber, string smsText,bool sendToAdmin=true)
        {
            var issmsEnabled = ConfigurationManager.AppSettings["IsSMSEnabled"];
            // var issmsEnabled =Convert.ToBoolean(ConfigurationManager.AppSettings["IsSMSEnabled"]);
            if (issmsEnabled=="true" && sendToAdmin)
                mobileNumber = "01739110321+" + mobileNumber;
            else
                mobileNumber = "01739110321";
             return this.sendSMSByMIM(mobileNumber, smsText);
            return "";
        }


        public string sendSMSByMIM(string mobileNumber, string smsText)
        {
            RestClient client = new RestClient(string.Concat("http://brandsms.mimsms.com/smsapi?api_key=C20023855be6de9ed538d9.60239701&type=text&contacts=", mobileNumber, "&senderid=8804445629106"));
            RestClientExtensions.AddDefaultParameter(client, "msg", smsText);
            RestRequest request = new RestRequest();
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            return client.Execute(request).Content;
        }
    }
}