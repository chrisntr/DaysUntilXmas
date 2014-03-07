using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Graphics;
using Android.Util;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using TimeLibrary;
using DaysUntilChristmasAndroid;
using Android.Media;
using System.Text;
using System.Linq;
using DaysUntilXmasAndroid.Helpers;

namespace DaysUntilXmasAndroid
{
	[Activity (Label = "Days Until Xmas", MainLauncher = true, Theme="@style/TransparentTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait )]
	public class Activity1 : FragmentActivity
	{
		private ScreenSlidePagerAdapter adapter;
		private ViewPager pager;
		private IPageIndicator indicator;
		private TextView titleDays;
		readonly TimeInformation _time = new TimeInformation();
		private System.Timers.Timer timer;
		MediaPlayer player;
		private const int MuteOption = 5;
		private MusicOptions musicOptions = new MusicOptions ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);
			ActionBar.Title = string.Empty;

			Typeface face= Typeface.CreateFromAsset(Assets,"fonts/segoeui.ttf");


			var titleText = FindViewById<TextView> (Resource.Id.titleText);
			titleDays = FindViewById<TextView> (Resource.Id.titleDays);
			
			if (titleText != null && face != null) {
				titleText.Typeface = face;
				titleDays.Typeface = face;
			}

			adapter = new ScreenSlidePagerAdapter(SupportFragmentManager);

			pager = FindViewById<ViewPager>(Resource.Id.pager);
			pager.Adapter = adapter;

			indicator = FindViewById<TitlePageIndicator>(Resource.Id.indicator);
			indicator.SetViewPager(pager);
			pager.OffscreenPageLimit = (adapter.Count);

			timer = new System.Timers.Timer (1000);
			timer.Elapsed += HandleElapsed;
				
			_time.PropertyChanged += (sender, e) => {
				if(e.PropertyName != "DaysUntil")
					return;
				RunOnUiThread(() => {
						titleDays.Text = _time.DaysUntil;
				});
			};

		}

		void HandleElapsed (object sender, System.Timers.ElapsedEventArgs e)
		{
			PopulateTimeInformation();
		}

		protected override void OnStart ()
		{
			base.OnStart ();
			timer.Start ();
			PlayAudio (Settings.MusicChoice);
		}

		protected override void OnStop ()
		{
			base.OnStop ();
			timer.Stop ();
			StopMusic ();
		}

		public void PlayAudio(int track)
		{
			if (Settings.MusicChoice != track)
				Settings.MusicTimeStamp = 0;

			Settings.MusicChoice = track;

			if (track == MuteOption) {
				StopMusic ();
			} else {
				PlayAudioTrack (musicOptions.MusicItems [track].ResId);
			}
		}

		public void StopMusic()
		{
			if (player == null)
				return;
			Settings.MusicTimeStamp = player.CurrentPosition;
			player.Stop ();
			player.SetVolume (0, 0);
			player = null;
		}

		public void PlayAudioTrack(int id)
		{
			if (Settings.MusicChoice == MuteOption)
				return;

			if (player != null && player.IsPlaying)
				player.Stop ();

			player = MediaPlayer.Create (this, id);
			player.SeekTo(Settings.MusicTimeStamp);
			player.Start ();

			player.Completion += (object sender, EventArgs e) => {
				Settings.MusicTimeStamp = 0;
				player.SeekTo(Settings.MusicTimeStamp);
				player.Start();
			};

		}



		void PopulateTimeInformation ()
		{
			TimeSpan timeDifference = TimeHelper.GetTimeDifference ();

			_time.DaysUntil = String.Format ("{0:0,0}", (int)timeDifference.TotalDays + 1);

			if (DateTime.Now.Day == 25 && DateTime.Now.Month == 12) {
				_time.DaysUntil = "Today";
			}
		}

		void PopulateAllTimeInformation ()
		{
			TimeSpan timeDifference = TimeHelper.GetTimeDifference ();

			_time.DaysUntil = String.Format ("{0:0,0}", (int)timeDifference.TotalDays + 1);

			if (DateTime.Now.Day == 25 && DateTime.Now.Month == 12) {
				_time.DaysUntil = "Today";
				_time.SecondsUntil = "Today";
				_time.MinutesUntil = "Today";
				_time.HoursUntil = "Today";
			} else {
				_time.SecondsUntil = String.Format ("{0:0,0}", (int)timeDifference.TotalSeconds);
				_time.MinutesUntil = String.Format ("{0:0,0}", (int)timeDifference.TotalMinutes);
				_time.HoursUntil = String.Format ("{0:0,0}", (int)timeDifference.TotalHours);
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{

			MenuInflater.Inflate (Resource.Menu.menu_main, menu);

			return true;

		}

		private string GetShareText()
		{
			PopulateAllTimeInformation ();
			if (DateTime.Now.Day == 25 && DateTime.Now.Month == 12) {
				return "Today is Christmas! Get the app from http://daysuntilxmas.com #daysuntilxmas";
			}

			var pageNumber = pager.CurrentItem;
			var time = string.Empty;
			var unit = string.Empty;
			switch (pageNumber) {
			case 0:
				time = _time.DaysUntil;
				unit = (_time.DaysUntil == "01") ? "day": "days";
				break;
			case 1:
				time = _time.HoursUntil;
				unit = (_time.HoursUntil == "01") ? "hour": "hours";
				break;
			case 2:
				time = _time.MinutesUntil;
				unit = (_time.MinutesUntil == "01") ? "minute": "minutes";
				break;
			case 3:
				time = _time.SecondsUntil;
				unit = (_time.SecondsUntil == "01") ? "second" : "seconds";
				break;
			}
			var sb = new StringBuilder ();
			sb.Append(String.Format("Only {0} {1} left until Christmas! Get the app from {2} #daysuntilxmas", time.ToString(), unit, "http://daysuntilxmas.com"));
			return sb.ToString ();
		}
		

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			Console.WriteLine (item);
			switch (item.ItemId) {
			case Resource.Id.menu_music:
				{
					// 1. Instantiate an AlertDialog.Builder with its constructor
					AlertDialog.Builder builder = new AlertDialog.Builder (this);

					// 2. Chain together various setter methods to set the dialog characteristics
					builder//.SetMessage("Dialog message")
						.SetTitle ("Select Music")
						.SetSingleChoiceItems (musicOptions.MusicItems.Select(m => m.Name).ToArray(), Settings.MusicChoice, (s, e) => {
							PlayAudio(e.Which);
						})
						.SetNeutralButton ("Done", delegate(object sender, DialogClickEventArgs e)  {});
						
					AlertDialog dialog = builder.Create ();
					dialog.Show ();
				}
				return true;
			case Resource.Id.menu_about:
				{
					// 1. Instantiate an AlertDialog.Builder with its constructor
					AlertDialog.Builder builder = new AlertDialog.Builder (this);

					// 2. Chain together various setter methods to set the dialog characteristics
					builder//.SetMessage("Dialog message")
						.SetTitle ("About Days Until Xmas")
						.SetMessage ("Days Until Xmas is an app created by Chris Hardy (@ChrisNTR) and James Montemagno (@JamesMontemagno) in C# with Xamarin: www.xamarin.com. Music created by Kevin MacLeod - incompetech.com");

					// 3. Get the AlertDialog from create()
					AlertDialog dialog = builder.Create ();
					dialog.Show ();
				}
				return true;
			case Resource.Id.menu_share:
				var intent = new Intent(Intent.ActionSend);
				intent.PutExtra(Intent.ExtraText,GetShareText());
				intent.SetType("text/plain");
				StartActivity(Intent.CreateChooser(intent, Resources.GetString(Resource.String.menu_share)));
				return true;
			}
		
			return base.OnOptionsItemSelected (item);
		}

		public override void OnBackPressed ()
		{

			if (pager.CurrentItem == 0) {
				// If the user is currently looking at the first step, allow the system to handle the
				// Back button. This calls finish() on this activity and pops the back stack.
				base.OnBackPressed();
			} else {
				// Otherwise, select the previous step.
				pager.CurrentItem = (pager.CurrentItem - 1);
			}
		}
	}
}


