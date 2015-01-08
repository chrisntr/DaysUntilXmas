
using Android.App;
using Android.OS;
using Android.Support.Wearable.Views;

namespace DaysUntilXmasAndroid
{
	[Activity (Label = "Days Until Xmas", MainLauncher = true, Icon = "@drawable/ic_launcher")]
	public class MainActivity : Activity
	{

		GridViewPager viewPager;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			var v = FindViewById<WatchViewStub> (Resource.Id.watch_view_stub);
			v.LayoutInflated += delegate {

				viewPager = FindViewById<GridViewPager>(Resource.Id.pager);
				var pager = new TimeLeftGridPagerAdapter(FragmentManager);
				viewPager.Adapter = pager;
				viewPager.OffscreenPageCount = viewPager.Adapter.RowCount;
			};
		}
	}
}



