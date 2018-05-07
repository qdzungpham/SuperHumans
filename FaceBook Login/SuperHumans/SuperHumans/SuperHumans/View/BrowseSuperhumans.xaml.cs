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
	public partial class BrowseSuperhumans : ContentPage
	{
		public BrowseSuperhumans ()
		{
			InitializeComponent ();
            this.BindingContext = new SuperHumans.ViewModel.BrowseSuperhumans();
            
            //when loading is done.... delete must be invisible, How can I get a message that

            // Layout including Pregressing Bar should be removed or be invisible
            //var BSVM = BindingContext as SuperHumans.ViewModel.BrowseSuperhumans;
            //if(BSVM != null)
            //{
            //    if(BSVM.IsBusy == false) // Remove Progressing Bar.
            //    {
            //        delete.IsVisible = false;
            //    }
            //}


        }
	}
}