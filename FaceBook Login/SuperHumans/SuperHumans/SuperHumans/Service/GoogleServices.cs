using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperHumans.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.Service
{
    public class GoogleServices
    {

        /// <summary>
        /// Create a new app and get new creadentials: 
        /// https://console.developers.google.com/apis/
        /// </summary>
        public static readonly string ClientId = "947608877670-t7394f94ego7oh1aqupt2vthrvt0hotv.apps.googleusercontent.com";
        public static readonly string ClientSecret = "JOgAe9diKA1vQ0B-BIG_an7b";
        public static readonly string RedirectUri = "https://www.youtube.com/feed/history";

        public async Task<string> GetAccessTokenAsync(string code)
        {
            var requestUrl =
                "https://www.googleapis.com/oauth2/v4/token"
                + "?code=" + code
                + "&client_id=" + ClientId
                + "&client_secret=" + ClientSecret
                + "&redirect_uri=" + RedirectUri
                + "&grant_type=authorization_code";

            var httpClient = new HttpClient();

            var response = await httpClient.PostAsync(requestUrl, null);

            var json = await response.Content.ReadAsStringAsync();

            var accessToken = JsonConvert.DeserializeObject<JObject>(json).Value<string>("access_token");

            return accessToken;
        }

        public async Task<GoogleProfile> GetGoogleUserProfileAsync(string accessToken)
        {

            var requestUrl = "https://www.googleapis.com/plus/v1/people/me"
                             + "?access_token=" + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var googleProfile = JsonConvert.DeserializeObject<GoogleProfile>(userJson);

            return googleProfile;
        }
    }
}
