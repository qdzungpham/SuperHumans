using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using SuperHumans.Droid.Activities;

namespace SuperHumans.Droid.Fragments
{
    public class HomeFragment : Fragment
    {

        ViewPager pager;
        TabsAdapter adapter;
        FloatingActionButton fab;
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
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment_home, null);

            pager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            adapter = new TabsAdapter(Activity, ChildFragmentManager);
            var tabs = view.FindViewById<TabLayout>(Resource.Id.tabs);
            pager.Adapter = adapter;
            tabs.SetupWithViewPager(pager);
            pager.OffscreenPageLimit = 3;

            pager.PageSelected += (sender, args) =>
            {
                var fragment = adapter.InstantiateItem(pager, args.Position) as IFragmentVisible;

                fragment?.BecameVisible();
            };

            fab.Click += (sender, e) =>
            {
                var intent = new Intent(Activity, typeof(AskActivity)); ;
                StartActivity(intent);
            };
            return view;
        }
    }

    class TabsAdapter : FragmentStatePagerAdapter
    {
        string[] titles;

        public override int Count => titles.Length;

        public TabsAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            titles = context.Resources.GetTextArray(Resource.Array.sections);
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position) =>
                            new Java.Lang.String(titles[position]);

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0: return MyFeedFragment.NewInstance();
                case 1: return MyFeedFragment.NewInstance();
            }
            return null;
        }

        public override int GetItemPosition(Java.Lang.Object frag) => PositionNone;
    }
}