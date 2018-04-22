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
	public partial class Logout : ContentPage
	{
		public Logout ()
		{
			InitializeComponent ();
            this.BindingContext = new Account();
		}

        public async void LogoutButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }
    }
}