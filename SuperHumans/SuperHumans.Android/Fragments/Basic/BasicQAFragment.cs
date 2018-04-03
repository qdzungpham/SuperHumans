using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicQAFragment : Fragment
    {
        Button browse, ask;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

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

            browse = view.FindViewById<Button>(Resource.Id.browse);
            ask = view.FindViewById<Button>(Resource.Id.ask);

            browse.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicBrowseQuestionsFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };


            return view;
        }
    }
}