using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace SuperHumans.Droid.Fragments
{
    public class AccountFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
 
            HasOptionsMenu = true;
        }

        public static AccountFragment NewInstance()
        {
            var accountFragment = new AccountFragment { Arguments = new Bundle() };
            return accountFragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.fragment_account, null);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.account_top_menus, menu);
            
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }
    }
}