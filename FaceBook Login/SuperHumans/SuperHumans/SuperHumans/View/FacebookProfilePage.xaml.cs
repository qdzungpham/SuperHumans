using SuperHumans.Service;
using SuperHumans.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SuperHumans.View
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacebookProfilePage : ContentPage
    {
        private string ClientId = "425439717877690";

        public FacebookProfilePage()
        {
            InitializeComponent();
            //this.BindingContext = new FacebookViewModel();

            var apiRequest =
                "https://www.facebook.com/dialog/oauth?client_id="
                + ClientId
                + "&response_type=token&redirect_uri=https://www.facebook.com/connect/login_success.html";

            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;

            Content = webView;
        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var accessToken = ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                var vm = BindingContext as FacebookViewModel;
                await vm.SetFacebookUserProfileAsync(accessToken);
                var name = vm.FacebookProfile.Name;
                var ID = vm.FacebookProfile.Id;

                Content = MainStackLayout;

                var AuthService = DependencyService.Get<IAuthService>();
                AuthService.SaveCredentials("FaceBook", ID, "facebook"); // 어떻게 facebook으로부터ID를 가져올수 있는가?
                Application.Current.Properties["UserName"] = ID; // 임시적으로 이렇게 해놔 ... 고쳐야함
                Application.Current.Properties["Name"] = name;
                await Navigation.PushAsync(new MainNavi());
            }
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

                //if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                //{
                //    at = url.Replace("http://www.facebook.com/connect/login_success.html#access_token=", "");
                //}

                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }
    }
}