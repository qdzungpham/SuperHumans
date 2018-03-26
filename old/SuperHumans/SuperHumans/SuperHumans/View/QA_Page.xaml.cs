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
	public partial class QA_Page : ContentPage
	{
		public QA_Page ()
		{
			InitializeComponent ();
		}

        void PreviousPageButton(object sender,EventArgs e)
        {
            Navigation.PopAsync();
        } // 

        void GoAskQuestionButton(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AskQA());
        }

        void GoBrowseQButton(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BrowseQuestions());
        }

    }
}