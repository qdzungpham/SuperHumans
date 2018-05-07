using SuperHumans.Model;
using SuperHumans.ModelContext;
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
	public partial class BrowseQuestions : ContentPage
	{
		public BrowseQuestions ()
		{
			InitializeComponent ();
            this.BindingContext = new SuperHumans.ViewModel.BrowseQuestions();

        }

        //async void OnTapped(object sender, ItemTappedEventArgs e)
        //{
        //    var SelectedQuestion = QList.SelectedItem as Question;
        //    //DisplayAlert("Item Tapped", Selected.Name, "Okay");
        //    if(SelectedQuestion != null)
        //    {
        //        var BQVM = BindingContext as SuperHumans.ViewModel.BrowseQuestions;
        //        if(BQVM != null)
        //        {
        //            BQVM.SelectedQuestion = SelectedQuestion;

        //            await Navigation.PushAsync(new EditableQuestionPage(BQVM));
        //        }
        //    }
        //    //Remove selected colour when touched
        //    ((ListView)sender).SelectedItem = null;
        //}

        async void OnTapped(object sender, ItemTappedEventArgs e)
        {
            var SelectedQuestion = QList.SelectedItem as Question;
            //DisplayAlert("Item Tapped", Selected.Name, "Okay");
            if (SelectedQuestion != null)
            {
                var BQVM = BindingContext as SuperHumans.ViewModel.BrowseQuestions;
                if (BQVM != null)
                {
                    BQVM.SelectedQuestion = SelectedQuestion;
                    
                    await Navigation.PushAsync(new QuestionDetailPage(BQVM));
                    await BQVM.GetAnswersByIDAsnyc(SelectedQuestion.QuestionID.ToString());
                }
            }
            //Remove selected colour when touched
            ((ListView)sender).SelectedItem = null;
        }
    }
}