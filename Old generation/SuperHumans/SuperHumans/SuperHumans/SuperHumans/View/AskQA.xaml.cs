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
	public partial class AskQA : ContentPage
	{
		public AskQA ()
		{
			InitializeComponent ();
            this.BindingContext = new SuperHumans.ViewModel.AskQA();
            MessagingCenter.Subscribe<SuperHumans.ViewModel.AskQA,string> (this, "AskQMssg", async(sender, e) =>
            {
                await Navigation.PushAsync(new BrowseQuestions());
            });
        }
	}
}