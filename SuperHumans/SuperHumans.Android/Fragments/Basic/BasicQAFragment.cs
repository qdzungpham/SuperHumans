using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SuperHumans.Droid.Activities;
using SuperHumans.Droid.Activities.Basic;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicQAFragment : Fragment
    {
        Button browse, ask;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            HasOptionsMenu = true;
        }

        public static BasicQAFragment NewInstance()
        {
            var fragment = new BasicQAFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_QA, null);

            Activity.Title = "Opportunities";

            browse = view.FindViewById<Button>(Resource.Id.browse);
            ask = view.FindViewById<Button>(Resource.Id.ask);

            browse.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicBrowseQuestionsFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };

            ask.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicAskQuestionFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };


            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.speak_menu, menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_speak:

                    ((BasicMainActivity)Activity).Speak("Hello, how are you today?");

                    break;


            }
            return base.OnOptionsItemSelected(item);
        }
    }
}