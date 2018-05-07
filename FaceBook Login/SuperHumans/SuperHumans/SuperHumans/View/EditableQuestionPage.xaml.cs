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
	public partial class EditableQuestionPage : ContentPage
	{
		public EditableQuestionPage ()
		{
			InitializeComponent ();
            this.BindingContext = new SuperHumans.ViewModel.BrowseQuestions();
        }

        public EditableQuestionPage(SuperHumans.ViewModel.BrowseQuestions BQVM)
        {
            InitializeComponent();
            BindingContext = BQVM;
        }
    }
}