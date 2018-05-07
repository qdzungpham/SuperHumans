using SuperHumans.Model;
using SuperHumans.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ViewModel
{
    public class GoogleViewModel : BaseViewModel
    {
        private GoogleProfile _googleProfile;
        private readonly GoogleServices _googleServices;

        public GoogleProfile GoogleProfile
        {
            get { return _googleProfile; }
            set
            {
                _googleProfile = value;
                SetProperty(ref _googleProfile, value);
            }
        }

        public GoogleViewModel()
        {
            _googleServices = new GoogleServices();
        }

        public async Task<string> GetAccessTokenAsync(string code)
        {

            var accessToken = await _googleServices.GetAccessTokenAsync(code);

            return accessToken;
        }

        public async Task SetGoogleUserProfileAsync(string accessToken)
        {

            GoogleProfile = await _googleServices.GetGoogleUserProfileAsync(accessToken);
        }

    }
}
