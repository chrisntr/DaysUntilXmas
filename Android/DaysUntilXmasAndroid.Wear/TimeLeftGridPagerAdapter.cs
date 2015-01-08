
using Android.Support.Wearable.Views;
using Android.App;

namespace DaysUntilXmasAndroid
{
	public class TimeLeftGridPagerAdapter : FragmentGridPagerAdapter
	{
		private static readonly string[] Content = {"DAYS", "HOURS", "MINUTES", "SECONDS"};
	
		private int _count = Content.Length;

		public TimeLeftGridPagerAdapter(FragmentManager p0) 
			: base(p0) 
		{
		}

		public override int GetColumnCount (int row)
		{
			return 1;
		}

		public override int RowCount {
			get {
				return _count;
			}
		}

		public override Android.App.Fragment GetFragment (int row, int column)
		{
			return TimeLeftFragment.NewInstance(row);
		}



		public void SetCount(int count)
		{
			if (count <= 0 || count > 10) return;

			_count = count;
			NotifyDataSetChanged();
		}
	}
}

