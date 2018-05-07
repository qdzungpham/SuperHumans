using Parse;
//using Plugin.AutoLogin;
using SuperHumans.Helper;
using SuperHumans.Service;
using SuperHumans.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SuperHumans
{
	public partial class App : Application
	{
        //private static ParseAccess db;

		public App ()
		{
			InitializeComponent();
            MainPage = new NavigationPage(new Login());

            var AuthService = new AuthService();

            if (AuthService.UserName != null) MainPage = new NavigationPage(new MainNavi());
            else MainPage = new NavigationPage(new Login()); // Login

        }

        //public static ParseAccess DB
        //{
        //    get
        //    {
        //        if(db == null)
        //        {
        //            db = new ParseAccess();
        //        }
        //        return db;
        //    }
        //}

		protected override void OnStart ()
		{
            // Handle when your app starts
            //Initialize();
		}

        //private void Initialize()
        //{
        //    ServiceLocator.Instance.Register<IParseAccess, ParseAccess>();
        //    ServiceLocator.Instance.Register<IRestService, RestService>();
        //    try
        //    {
        //        ParseClient.Configuration config = new ParseClient.Configuration
        //        {
        //            ApplicationId = "SuperMen",
        //            WindowsKey = "rickpham",
        //            Server = @"http://ec2-52-63-31-67.ap-southeast-2.compute.amazonaws.com:1337/parse/"
        //        };
        //        ParseClient.Initialize(config);
        //    }
        //    catch (Exception e)
        //    {

        //        Debug.WriteLine("ERRORR ****************************");
        //        Debug.WriteLine(e.ToString());
        //    }
        //}

        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
