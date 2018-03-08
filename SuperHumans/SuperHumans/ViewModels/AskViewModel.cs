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

        public AskViewModel()
        {
            Title = "Ask Question";
            PostCommand = new Command<Question>(async (Question question) => await Post(question));
        }

        private async Task Post(Question question)
        {
            try
            {
                await ParseAccess.AddQuestion(question);
                UserDialogs.Instance.Toast("Posted.", TimeSpan.FromSeconds(3));
            }
            catch (Exception e)
            {
                UserDialogs.Instance.Alert(e.Message, "ERROR", "OK");
            }
        }
    }
}
