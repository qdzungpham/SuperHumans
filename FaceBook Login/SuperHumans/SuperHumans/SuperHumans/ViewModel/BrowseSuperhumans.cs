using SuperHumans.Model;
using SuperHumans.ModelContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SuperHumans.ViewModel
{
    public class BrowseSuperhumans : BaseViewModel
    {
        public BrowseSuperhumans()
        {
            SuperhumanList = new ObservableCollection<SuperHuman>();
            GetSuperhumansAsync();
        }

        private async void GetSuperhumansAsync() // Task<ObservableCollection<Question>>
        {
            //var questions = await App.DB.LoadQuestions();
            IsBusy = true;
            var SService = new SuperhumanContext();
            var superhumans = await SService.GetSuperHumansAsync("SuperhumenWebAPI"); // SuperhumenWebAPI
            foreach (var superhuman in superhumans)
            {
                var S = new SuperHuman
                {
                    SuperHumanID = superhuman.SuperHumanID,
                    Major = superhuman.Major, // This is enum data type.
                    Rating = superhuman.Rating,
                    SelfIntro = superhuman.SelfIntro,
                    UserID = superhuman.UserID
                };
                SuperhumanList.Add(S);
            }
            IsBusy = false;
        }

        private ObservableCollection<SuperHuman> superhumanList;
        public ObservableCollection<SuperHuman> SuperhumanList
        {
            get { return superhumanList; }
            set
            {
                SetProperty(ref superhumanList, value);
            }
        }
    }
}
