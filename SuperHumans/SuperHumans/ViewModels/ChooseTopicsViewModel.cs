using Parse;
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

        public ChooseTopicsViewModel()
        {

        }

        public async Task LoadTopics()
        {
            try
            {
                var topics = await ParseAccess.LoadTopics();

                Topics = new List<ParseObject>();

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
    }
}
