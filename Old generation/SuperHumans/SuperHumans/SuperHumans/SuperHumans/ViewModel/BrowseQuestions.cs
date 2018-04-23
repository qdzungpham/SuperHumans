using SuperHumans.Model;
using SuperHumans.ModelContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            //var questions = await App.DB.LoadQuestions();
            IsBusy = true;
            var QService = new QuestionContext();
            var questions = await QService.GetQuestionsAsync("QuestionsWebAPI");
            foreach (var question in questions)
            {
                var q = new Question
                {
                    QuestionID = question.QuestionID,
                    Title = question.Title,
                    Body = question.Body,
                    Owner = question.Owner,
                    TimeAgo = question.TimeAgo
                };
                QuestionList.Add(q);
            }
            IsBusy = false;
        }

        public async Task GetAnswersByIDAsnyc(string _id)
        {
            var AService = new AnswerContext();
            var answers = await AService.GetAnswersAsync("AnswersWebAPI",_id);
            AnswerList = new ObservableCollection<Answer>();
            foreach (var answer in answers)
            {
                var a = new Answer
                {
                    AnswerID = answer.AnswerID,
                    Body = answer.Body,
                    CreatedBy = answer.CreatedBy,
                    TimeAgo = answer.TimeAgo,
                    QuestionID = Int32.Parse(_id)
                };
                AnswerList.Add(a);
            }
        }

        //private async void GetQuestionAsync(string id) // Task<ObservableCollection<Question>>
        //{
        //    var QService = new QuestionContext();
        //    var question = await QService.GetQuestionAsync("QuestionsWebAPI",id);
        //    QuestionList.Add(question);
        //}
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

        private ObservableCollection<Answer> answerList;
        public ObservableCollection<Answer> AnswerList
        {
            get { return answerList; }
            set
            {
                SetProperty(ref answerList, value);
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                SetProperty(ref message, value);
            }
        }

        private Question _q;
        public Question SelectedQuestion
        {
            get { return _q; }
            set
            {
                SetProperty(ref _q, value);
            }
        }
        
        public Command EditCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Message = "Edited successfully!" + _q.QuestionID + " , " + _q.Body + " , " + _q.Title + " , " + _q.TimeAgo + " , " + _q.Owner;
                    var QC = new QuestionContext();
                    await QC.EditQuestion(SelectedQuestion.QuestionID, SelectedQuestion, "QuestionsWebAPI");
                });
            }
        }

        public Command DeleteCommand
        {
            get
            {
                return new Command(async(x) =>
                {
                    Message = "Deleted successfully!";
                    var QC = new QuestionContext();
                    await QC.DeleteQuestion(_q.QuestionID, "QuestionsWebAPI");
                });
            }
        }

        public ICommand ItemClickCommand
        {
            get
            {
                return new Command((item) =>
                {
                    var q = item as Question;
                    Message = q.QuestionID.ToString();
                });
            }
        }
    }

    //public class BetterListView : ListView
    //{
    //    public static BindableProperty ItemClickCommandProperty = BindableProperty.Create(nameof(ItemClickCommand), typeof(ICommand), typeof(BetterListView), null);

    //    public ICommand ItemClickCommand
    //    {
    //        get
    //        {
    //            return (ICommand)this.GetValue(ItemClickCommandProperty);
    //        }
    //        set
    //        {
    //            this.SetValue(ItemClickCommandProperty, value);
    //        }
    //    }
    //    public BetterListView()
    //    {
    //        this.ItemTapped += OnItemTapped;
    //    }

    //    private void OnItemTapped(object sender, ItemTappedEventArgs e)
    //    {
    //        if(e.Item != null)
    //        {
    //            ItemClickCommand?.Execute(e.Item);
    //            SelectedItem = null;
    //        }
    //    }
    //}
}
