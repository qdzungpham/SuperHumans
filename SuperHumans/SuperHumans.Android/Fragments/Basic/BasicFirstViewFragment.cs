using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace SuperHumans.Droid.Fragments.Basic
{
    public class BasicFirstViewFragment : Fragment
    {
        Button QA, superHumans;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public static BasicFirstViewFragment NewInstance()
        {
            var fragment = new BasicFirstViewFragment { Arguments = new Bundle() };
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.basic_fragment_first_view, null);

            QA = view.FindViewById<Button>(Resource.Id.qa);
            superHumans = view.FindViewById<Button>(Resource.Id.superhumans);

            QA.Click += (sender, e) =>
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, BasicQAFragment.NewInstance())
                .AddToBackStack(null).Commit();
            };
            

            return view;
        }
    }
}