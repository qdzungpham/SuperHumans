using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using SuperHumans.Models;
using SuperHumans.ViewModels;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicAskQuestionFragment : Fragment
    {
        private readonly int VOICE = 10;
        TextView topics;
        //Button btnRec;
        //Button btnUseKeyboard;
        EditText titleText, bodyText;

        public AskViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            ViewModel = new AskViewModel();

            HasOptionsMenu = true;
        }

        public static BasicAskQuestionFragment NewInstance()
        {
            var fragment = new BasicAskQuestionFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_ask, null);

            Activity.Title = "Post New Task";

            
            titleText = view.FindViewById<EditText>(Resource.Id.ask_edit_title);
            bodyText = view.FindViewById<EditText>(Resource.Id.ask_edit_body);
            topics = view.FindViewById<TextView>(Resource.Id.topics);

            topics.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicChooseTopicsFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

            topics.Text = ViewModel.GetChoosedTopicsString();

            //// get the resources from the layout
            //btnRec = view.FindViewById<Button>(Resource.Id.btn_use_speech_to_text);

            // check to see if we can actually record - if we can, assign the event to the button
            //string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            //if (rec != "android.hardware.microphone")
            //{
            //    // no microphone, no recording. Disable the button and output an alert
            //    var alert = new AlertDialog.Builder(titleText.Context);
            //    alert.SetTitle("You don't seem to have a microphone to record with");
            //    alert.SetPositiveButton("OK", (sender, e) =>
            //    {
            //        titleText.Text = "No microphone present";
            //        titleText.Enabled = false;
            //        return;
            //    });

            //    alert.Show();
            //}
            //else
            titleText.LongClick += delegate
                {

                    if (titleText.IsFocused == false) return;
                    // create the intent and start the activity
                    var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                    // put a message on the modal dialog
                    voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");

                    // if there is more then 1.5s of silence, consider the speech over
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                    // you can specify other languages recognised here, for example
                    // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
                    // if you wish it to recognise the default Locale language and German
                    // if you do use another locale, regional dialects may not be recognised very well

                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                    try
                    {
                        StartActivityForResult(voiceIntent, VOICE);
                    }
                    catch (ActivityNotFoundException)
                    {

                        Toast.MakeText(Context, "Sorry! Your device doesn\'t support speech input", ToastLength.Short).Show();

                    }

                };

            return view;
        }


        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.ask_menus, menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_post:
                    var question = new Question
                    {
                        Title = titleText.Text,
                        Body = bodyText.Text
                    };

                    ViewModel.PostCommand.Execute(question);
                    FragmentManager.PopBackStack();
                    break;



            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == VOICE)
            {
                if (resultCode == -1)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = matches[0];

                        // limit the output to 500 characters
                        if (textInput.Length > 500)
                            textInput = textInput.Substring(0, 500);
                        titleText.Text = textInput;
                    }
                    else
                        titleText.Text = "No speech was recognised";

                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }



    }

}