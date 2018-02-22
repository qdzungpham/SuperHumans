using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace SuperHumans.Droid.Fragments
{
    public class HomeFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here


        }

        public static HomeFragment NewInstance()
        {
            var homeFragment = new HomeFragment { Arguments = new Bundle() };
            return homeFragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.fragment_home, null);
        }
    }
}