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
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GoogleProfilePage : ContentPage
	{
        private readonly GoogleViewModel _googleViewModel;

        public GoogleProfilePage()
        {
            InitializeComponent();

            _googleViewModel = BindingContext as GoogleViewModel;

            var authRequest =
                  "https://accounts.google.com/o/oauth2/v2/auth"
                  + "?response_type=code"
                  + "&scope=openid"
                  + "&redirect_uri=" + GoogleServices.RedirectUri
                  + "&client_id=" + GoogleServices.ClientId;

            var webView = new WebView
            {
                Source = authRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;

            Content = webView;
        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var code = ExtractCodeFromUrl(e.Url);

            if (code != "")
            {

                var accessToken = await _googleViewModel.GetAccessTokenAsync(code);

                await _googleViewModel.SetGoogleUserProfileAsync(accessToken);
                var vm_profile = _googleViewModel.GoogleProfile;
                var name = vm_profile.Name;
                var ID = vm_profile.Id;

                var AuthService = DependencyService.Get<IAuthService>();
                AuthService.SaveCredentials("Google", ID, "google"); // 어떻게 facebook으로부터ID를 가져올수 있는가?
                Application.Current.Properties["UserName"] = ID; // 임시적으로 이렇게 해놔 ... 고쳐야함
                Application.Current.Properties["Name"] = name;
                await Navigation.PushAsync(new MainNavi());
            }
        }

        private string ExtractCodeFromUrl(string url)
        {
            if (url.Contains("code="))
            {
                var attributes = url.Split('&');

                var code = attributes.FirstOrDefault(s => s.Contains("code=")).Split('=')[1];

                return code;
            }

            return string.Empty;
        }
    }
}