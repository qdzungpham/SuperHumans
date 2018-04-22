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
	public partial class Superhuman : ContentPage
	{
		public Superhuman ()
		{
			InitializeComponent ();
		}
        void PreviousPageButton(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        void BrowseSuperHumanButton(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BrowseSuperhumans());
        }
    }
}