using Android.OS;
using Android.Runtime;
using Android.Speech.Tts;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using static Android.Speech.Tts.TextToSpeech;

namespace SuperHumans.Droid.Activities
{
    public class BasicBaseActivity : AppCompatActivity, IOnInitListener
    {
        TextToSpeech textToSpeech;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutResource);
            Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);

            }

            textToSpeech = new TextToSpeech(this, this, "com.google.android.tts");

            // set the speed and pitch
            //textToSpeech.SetPitch(.5f);
            //textToSpeech.SetSpeechRate(.5f);
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                textToSpeech.SetLanguage(Java.Util.Locale.Default);
            }
        }

        public void Speak(string text)
        {
            textToSpeech.Speak(text, QueueMode.Flush, null, null);
        }

        public Toolbar Toolbar
        {
            get;
            set;
        }

        protected virtual int LayoutResource
        {
            get;
        }

        protected int ActionBarIcon
        {
            set { Toolbar?.SetNavigationIcon(value); }
        }
    }
}
