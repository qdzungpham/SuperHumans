using SuperHumans.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SuperHumans.ViewModel // BrowseQuestions
{
    public class BrowseQuestions : BaseViewModel
    {
        public BrowseQuestions()
        {
            QuestionList = new ObservableCollection<Question>()
            {
                new Question { ObjectId = "0", Title = "Test Title0", Body = "Test Body", TimeAgo = "now", Owner = "Noah" },
                new Question { ObjectId = "1", Title = "Test Title1", Body = "Test Body", TimeAgo = "now", Owner = "Noah" },
                new Question { ObjectId = "2", Title = "Test Title2", Body = "Test Body", TimeAgo = "now", Owner = "Noah" },
                new Question { ObjectId = "3", Title = "Test Title3", Body = "Test Body", TimeAgo = "now", Owner = "Noah" }
            };
        }

        private ObservableCollection<Question> GetQuestions()
        {
            return QuestionList;
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

        public Command QuestionListCommand
        {
            get
            {
                return new Command(x =>
                {
                    // 질문: QuestionList의 Add는 QuestionList의 OnPropertyChanged?을 Inovoke? 할 수 있는가?
                    QuestionList.Add(new Question { ObjectId = "4", Title = "Test Title3", Body = "Test Body", TimeAgo = "now", Owner = "Noah" });
                });
            }
        }
        //public ICommand RefreshCommand { private set; get; }
    }
}
