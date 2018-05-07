using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.Services
{
    public class RestService : IRestService
    {
        private RestClient client;
        public TimeSpan TimeDiff { get; private set; }
        public RestService()
        {
            client = new RestClient("http://ec2-52-63-31-67.ap-southeast-2.compute.amazonaws.com:1337/api/");
            DateTime serverNow = GetServerTime();
            TimeDiff = DateTime.Now - serverNow;
        }

        public DateTime GetServerTime()
        {
            var request = new RestRequest("getServerTime", Method.GET);
            IRestResponse response = client.Execute(request);
            //string str = "2018-03-12T10:19:55.766";
            var content = response.Content; // raw content as string
            //"\"2018-03-12T11:44:46.294Z\""
            var str = content;
            var charsToRemove = new string[] { "\\", "\"", "Z" };
            foreach (var c in charsToRemove)
            {
                str = str.Replace(c, string.Empty);
            }
            DateTime myDate = DateTime.ParseExact(str, "yyyy-MM-ddTHH:mm:ss.fff",
                                       System.Globalization.CultureInfo.InvariantCulture);
            //System.Diagnostics.Debug.WriteLine("***********************************");
            //System.Diagnostics.Debug.WriteLine(content);

            return myDate;
        }
    }
}
