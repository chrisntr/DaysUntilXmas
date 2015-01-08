using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TimeLibrary;
using Android.Graphics;

namespace DaysUntilXmasAndroid
{
	public class TimeLeftFragment : Fragment
	{
		private static readonly string[] Content = {"DAYS", "HOURS", "MINUTES", "SECONDS"};

		public static TimeLeftFragment NewInstance(int page)
		{
			var fragment = new TimeLeftFragment (page);
			return fragment;
		}

		/**
	     * The argument key for the page number this fragment represents.
	     */
		public String ARG_PAGE = "page";

		/**
	     * The fragment's page number, which is set to the argument value for {@link #ARG_PAGE}.
	     */
		private int pageNumber;

		readonly TimeInformation _time = new TimeInformation();

		[Preserve]
		public TimeLeftFragment(int page) : base()
		{
			pageNumber = page;
		}

		[Preserve]
		public TimeLeftFragment() : base()
		{
		}
		
		[Preserve]
		public TimeLeftFragment(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
		{
		}

		string unit;

		void ConvertUnit ()
		{
			switch (pageNumber) {
			case 0:
				unit = _time.DaysUntil;
				break;
			case 1:
				unit = _time.HoursUntil;
				break;
			case 2:
				unit = _time.MinutesUntil;
				break;
			case 3:
				unit = _time.SecondsUntil;
				break;
			}
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			if ((savedInstanceState != null) && savedInstanceState.ContainsKey(ARG_PAGE))
				pageNumber = savedInstanceState.GetInt(ARG_PAGE);
		}
		private FontFitTextView textView;
		private System.Timers.Timer timer;
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

//			tv.setTypeface(face);
			ViewGroup rootView = (ViewGroup)inflater.Inflate (Resource.Layout.FragmentScreenSlidePage, container, false);

			PopulateTimeInformation ();

			ConvertUnit ();

			Typeface face=Typeface.CreateFromAsset(Activity.Assets,"fonts/segoeui.ttf");

			textView = rootView.FindViewById <FontFitTextView> (Android.Resource.Id.Text1);
			textView.Text = unit;
			textView.SetTextColor (Color.White);
			textView.Typeface = face;
		
			var title = rootView.FindViewById<TextView> (Resource.Id.title);
			title.Typeface = face;
			title.SetTextColor (Color.White);
			title.Text = string.Format ("{0} until xmas", Content [pageNumber]);

			timer = new System.Timers.Timer (1000);
			timer.Elapsed += HandleElapsed;


			_time.PropertyChanged += (sender, e) => {
				Activity.RunOnUiThread(() => {
					ConvertUnit ();
					if (View != null)
					{
						textView.Text = unit;
					}
				});
			};

			return rootView;
		}

		void HandleElapsed (object sender, System.Timers.ElapsedEventArgs e)
		{
			PopulateTimeInformation();
		}

		public override void OnPause ()
		{
			base.OnPause ();
			timer.Stop ();
		}

		public override void OnResume ()
		{
			base.OnResume ();
			timer.Start ();
		}

		public override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);
			outState.PutInt(ARG_PAGE, pageNumber);
		}

		void PopulateTimeInformation ()
		{
			TimeSpan timeDifference = TimeHelper.GetTimeDifference ();

			_time.DaysUntil = String.Format ("{0:0,0}", (int)timeDifference.TotalDays + 1);

			if (DateTime.Now.Day == 25 && DateTime.Now.Month == 12) {
				_time.DaysUntil = "Today";
				_time.SecondsUntil = "Today";
				_time.MinutesUntil = "Today";
				_time.HoursUntil = "Today";
			} else {
				_time.SecondsUntil =  String.Format("{0:0,0}", (int)timeDifference.TotalSeconds);
				_time.MinutesUntil = String.Format("{0:0,0}", (int)timeDifference.TotalMinutes);
				_time.HoursUntil = String.Format("{0:0,0}", (int)timeDifference.TotalHours);
			}
			//daysUntilXmasLabel.Text = _time.DaysUntil;

		}

	}
}


