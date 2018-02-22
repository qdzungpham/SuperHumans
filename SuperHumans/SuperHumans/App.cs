using System;
using System.Diagnostics;
using Parse;
using SuperHumans.Helpers;
using SuperHumans.Models;
using SuperHumans.Services;

namespace SuperHumans
{
    public class App
    {

        public static void Initialize()
        {
            ServiceLocator.Instance.Register<IDataStore<Item>, MockDataStore>();
            ServiceLocator.Instance.Register<IParseAccess, ParseAccess>();
            try
            {
                ParseClient.Configuration config = new ParseClient.Configuration
                {
                    ApplicationId = "SuperMen",
                    WindowsKey = "rickpham",
                    Server = @"http://ec2-52-63-31-67.ap-southeast-2.compute.amazonaws.com:1337/parse/"
                };
                ParseClient.Initialize(config);
            }
            catch (Exception e)
            {

                Debug.WriteLine("ERRORR ****************************");
                Debug.WriteLine(e.ToString());
            }         
        }
    }
}
