﻿using Microsoft.AspNet.Identity;
using Microsoft.Owin.Builder;
using Owin;

namespace RiceMill_MVC
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(AppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
      
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
          
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();
        }
    }
}