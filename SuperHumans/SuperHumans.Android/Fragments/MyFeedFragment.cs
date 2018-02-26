using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace SuperHumans.Droid.Fragments
{
    public class MyFeedFragment : Fragment, IFragmentVisible
    {
        public static MyFeedFragment NewInstance() =>
            new MyFeedFragment { Arguments = new Bundle() };


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_myfeed, container, false);

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStop();
        }

        public void BecameVisible()
        {

        }

    }
}