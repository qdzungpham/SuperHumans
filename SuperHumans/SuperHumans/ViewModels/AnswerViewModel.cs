using Acr.UserDialogs;
using SuperHumans.Helpers;
using SuperHumans.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class AnswerViewModel : BaseViewModel
    {
        public Command PostCommand { get; private set; }

        bool isPosted = false;
        public bool IsPosted
        {
            get { return isPosted; }
            set { SetProperty(ref isPosted, value); }
        }
        
        public AnswerViewModel()
        {
            PostCommand = new Command<Answer>(async (Answer answer) => await ExecutePostAnswerAsync(answer));
        }

        public async Task ExecutePostAnswerAsync(Answer answer)
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;
                await ParseAccess.AddAnswer(answer);

                //UserDialogs.Instance.Toast("Posted.", TimeSpan.FromSeconds(3));

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
