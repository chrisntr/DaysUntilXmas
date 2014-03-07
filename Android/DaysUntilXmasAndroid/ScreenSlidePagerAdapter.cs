
using Android.Support.V4.App;

namespace DaysUntilXmasAndroid
{
	public class ScreenSlidePagerAdapter : FragmentPagerAdapter
	{
		private static readonly string[] Content = {"DAYS", "HOURS", "MINUTES", "SECONDS"};
	
		private int _count = Content.Length;

		public ScreenSlidePagerAdapter(Android.Support.V4.App.FragmentManager p0) 
			: base(p0) 
		{
		}

		public override int Count
		{
			get { return _count; }
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return ScreenSlidePageFragment.NewInstance(position);
		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted(int p0)
		{
			return new Java.Lang.String(Content[p0 % Content.Length]);
		}

		public void SetCount(int count)
		{
			if (count <= 0 || count > 10) return;

			_count = count;
			NotifyDataSetChanged();
		}
	}
}

