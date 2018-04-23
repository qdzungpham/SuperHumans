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
	public partial class NewUsersRegister : ContentPage
	{
		public NewUsersRegister ()
		{
			InitializeComponent ();
            this.BindingContext = new SuperHumans.ViewModel.Account();
		}
	}
}