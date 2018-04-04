using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicAskQuestionFragment : Fragment
    {
        private readonly int VOICE = 10;
        Button btnRec;
        Button btnUseKeyboard;
        EditText questionTitle;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
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

            Activity.Title = "Ask A Question";

            btnUseKeyboard = view.FindViewById<Button>(Resource.Id.btn_use_keyboard);
            questionTitle = view.FindViewById<EditText>(Resource.Id.ask_edit_title);

            btnUseKeyboard.Click += (sender, e) =>
            {

               
            };



            // get the resources from the layout
            btnRec = view.FindViewById<Button>(Resource.Id.btn_use_speech_to_text);

            // check to see if we can actually record - if we can, assign the event to the button
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // no microphone, no recording. Disable the button and output an alert
                var alert = new AlertDialog.Builder(btnRec.Context);
                alert.SetTitle("You don't seem to have a microphone to record with");
                alert.SetPositiveButton("OK", (sender, e) =>
                {
                    questionTitle.Text = "No microphone present";
                    btnRec.Enabled = false;
                    return;
                });

                alert.Show();
            }
            else
                btnRec.Click += delegate
                {

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
                        questionTitle.Text = textInput;
                    }
                    else
                        questionTitle.Text = "No speech was recognised";

                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }



    }
}