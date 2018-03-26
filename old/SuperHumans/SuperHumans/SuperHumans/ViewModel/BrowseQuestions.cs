using SuperHumans.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SuperHumans.ViewModel // BrowseQuestions
{
    public class BrowseQuestions : BaseViewModel
    {
        public BrowseQuestions()
        {
            QuestionList = new ObservableCollection<Question>();
            GetQuestionsAsync();
        }
        
        private async void GetQuestionsAsync() // Task<ObservableCollection<Question>>
        {
            var questions = await App.DB.LoadQuestions();
            foreach (var question in questions)
            {
                var q = new Question
                {
                    ObjectId = question.ObjectId,
                    Title = question.Get<string>("title"),
                    Body = question.Get<string>("body"),
                    //Owner = question.Get<String>("")
                    TimeAgo = "now"
                };
                QuestionList.Add(q);
            }
        }
        //어떻게 add하고 remove하고.. QuestionList가 그러한 작용에 반응하게 할 수 있는가?

        private ObservableCollection<Question> questionList;
        public ObservableCollection<Question> QuestionList
        {
            get { return questionList; }
            set
            {
                SetProperty(ref questionList, value);
            }
        }

        //public Command QuestionListCommand
        //{
        //    get
        //    {
        //        return new Command(x =>
        //        {
        //            // 질문: QuestionList의 Add는 QuestionList의 OnPropertyChanged?을 Inovoke? 할 수 있는가?
        //            QuestionList.Add(new Question { ObjectId = "4", Title = "Test Title3", Body = "Test Body", TimeAgo = "now", Owner = "Noah" });
        //        });
        //    }
        //}
        //public ICommand RefreshCommand { private set; get; }
    }
}
