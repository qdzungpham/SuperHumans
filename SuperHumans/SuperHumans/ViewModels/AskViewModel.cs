using Acr.UserDialogs;
using SuperHumans.Helpers;
using SuperHumans.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class AskViewModel : BaseViewModel
    {
        public Command PostCommand { get; private set; }

        bool isPosted = false;
        public bool IsPosted
        {
            get { return isPosted; }
            set { SetProperty(ref isPosted, value); }
        }

        public AskViewModel()
        {
            Title = "Ask Question";
            PostCommand = new Command<Question>(async (Question question) => await ExecutePostQuestionAsync(question));
        }

        public async Task ExecutePostQuestionAsync(Question question)
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                await ParseAccess.AddQuestion(question);
                
                UserDialogs.Instance.Toast("Posted.", TimeSpan.FromSeconds(3));

                IsPosted = true;
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message, "ERROR", "OK");
            }
            finally
            {
                IsBusy = false;

                ProgressDialogManager.DisposeProgressDialog();
            }
        }
    }
}
