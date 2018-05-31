using Parse;
using SuperHumans.Helpers;
using SuperHumans.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SuperHumans.ViewModels
{
    public class ChooseTopicsViewModel : BaseViewModel
    {
        public List<ParseObject> Topics { get; private set; }
        public string[] TopicStrings { get; private set; }

        public Command SaveFollowedTopicsCommand { get; private set; }

        public ChooseTopicsViewModel()
        {
            Topics = new List<ParseObject>();

            SaveFollowedTopicsCommand = new Command<List<int>>(async (List<int> topicIndices) => await ExecuteSaveFollowedTopicsCommandAsync(topicIndices));
        }

        public async Task LoadTopics()
        {
            try
            {
                var topics = await ParseAccess.LoadTopics();

                foreach (var topic in topics)
                {
                    Topics.Add(topic);
                }

                TopicStrings = new string[Topics.Count];

                for (var i = 0; i < Topics.Count; i++)
                {
                    TopicStrings[i] = Topics[i].Get<string>("topicText");
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task ExecuteSaveFollowedTopicsCommandAsync(List<int> topicIndices)
        {
            if (IsBusy) return;

            ProgressDialogManager.LoadProgressDialog("Loading...");

            try
            {
                IsBusy = true;

                var selectedTopics = new List<ParseObject>();
                foreach (var i in topicIndices)
                {
                    selectedTopics.Add(Topics[i]);
                }

                await ParseAccess.UpdateFollowedTopics(selectedTopics);
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
                ProgressDialogManager.DisposeProgressDialog();
            }
        }

        public void ChooseTopics(List<int> topicIndices)
        {
            foreach (var i in topicIndices)
            {
                AskViewModel.Topics.Add(Topics[i]);
            }
        }
    }
}
