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
	public partial class QuestionDetailPage : ContentPage
	{
		public QuestionDetailPage ()
		{
			InitializeComponent ();
            this.BindingContext = new SuperHumans.ViewModel.BrowseQuestions();
        }

        public QuestionDetailPage(SuperHumans.ViewModel.BrowseQuestions BQVM)
        {
            InitializeComponent();
            BindingContext = BQVM;
        }
    }
}