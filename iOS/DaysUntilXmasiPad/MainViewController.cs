using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using MonoTouch.AVFoundation;
using DaysUntilChristmas;
using TimeLibrary;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DaysUntilXmasiPad
{
	public partial class MainViewController : UIViewController
	{
		
		FlipsideViewController flipsideViewController;
		AboutViewController aboutViewController;
		UIPopoverController flipsidePopoverController, aboutPopoverController;
		UIActionSheet actionSheet;
		AVAudioPlayer audioPlayer;
		readonly TimeInformation _time = new TimeInformation();
		public MusicOptions musicOption = new MusicOptions();
		UIScrollView scrollView;
		UIPageControl pageControl;

		int scrollViewWidth = 700;
		int scrollViewHeight = 220;
		int scrollViewPositionX = 50;
		int scrollViewPositionY = 735;
		
		UILabel label, label2, label3, label4;

		public static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MainViewController () : base (UserInterfaceIdiomIsPhone ? "MainViewController_iPhone" : "MainViewController", null)
		{
			// Change scrollView position for iPhone
			if (UserInterfaceIdiomIsPhone) {
				scrollViewWidth = 480;
				scrollViewHeight = 120;
				scrollViewPositionX = 10;
				if (UIScreen.MainScreen.Bounds.Height > 480) {
					scrollViewPositionY = (int) UIScreen.MainScreen.Bounds.Height - scrollViewHeight - 25;
				} else {
					scrollViewPositionY = 330;
				}

			}
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			// Reposition days scroll view when view re-appears
			UpdatePageScroll (true);
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			AddNightFallGradient();

			AddButtons();

			SetUpMusic();

			NSTimer.CreateRepeatingScheduledTimer(1.0, () => {
				PopulateTimeInformation();
			});

			PopulateTimeInformation ();

			scrollView = new UIScrollView (new RectangleF (scrollViewPositionX, scrollViewPositionY, scrollViewWidth, scrollViewHeight));
			View.AddSubview (scrollView);

			scrollView.PagingEnabled = true;
			scrollView.Bounces = true;
			scrollView.DelaysContentTouches = true;
			scrollView.ShowsHorizontalScrollIndicator = true;
			
			scrollView.ContentSize = new System.Drawing.SizeF (scrollViewWidth * 4, scrollViewHeight);
			scrollView.ScrollRectToVisible (new RectangleF (0, 0, scrollViewWidth, scrollViewHeight), true);

			scrollView.DecelerationEnded += (object sender, EventArgs e) => {
				UpdatePageScroll(true);
			};

			SetUpLabels();


			pageControl = new UIPageControl();
			pageControl.Center = new PointF(UIScreen.MainScreen.Bounds.Width / 2f, UIScreen.MainScreen.Bounds.Height - 50f);//, UIScreen.MainScreen.Bounds.Height - 150f, 100f, 100f ));
			pageControl.Pages = 4;
			pageControl.CurrentPage = 0;
			if (!UserInterfaceIdiomIsPhone)
				View.AddSubview(pageControl);

			_time.PropertyChanged += (sender, e) => {
				daysUntilXmasLabel.Text = _time.DaysUntil;

				label.Text = _time.DaysUntil;
				label2.Text = _time.HoursUntil;
				label3.Text = _time.MinutesUntil;
				label4.Text = _time.SecondsUntil;
			};

		}

		void SetUpMusic ()
		{
			if(!SettingsHelper.Mute)
				PlayAudio();
		}

		public void MuteAudio (bool on)
		{
			SettingsHelper.Mute = on;
			if (on) {
				audioPlayer.Stop();
				audioPlayer.Volume = 0;
				audioPlayer.Dispose();
				audioPlayer = null;
			} else {
				PlayAudio();
			}
		}

		public void PlayAudio ()
		{
			var item = musicOption.MusicItems.FirstOrDefault(x => x.DefaultOption).Path;
			PlayAudio(item);
		}

		public void PlayAudio (string fileName)
		{
			if (!SettingsHelper.Mute) {
				if (audioPlayer != null && audioPlayer.Playing)
					audioPlayer.Stop ();
				var filePath = NSUrl.FromFilename (fileName);
				var newPlayer = AVAudioPlayer.FromUrl (filePath);
				newPlayer.PrepareToPlay ();
				audioPlayer = newPlayer;
				audioPlayer.Volume = 1;
				audioPlayer.Play ();

				audioPlayer.FinishedPlaying += (sender, e) => {
					audioPlayer.Play ();
				};
			}
		}

		void SetUpLabels ()
		{
			var fontName = "Open Sans";

			if (UserInterfaceIdiomIsPhone) {
				daysUntilXmasLabel.Font = UIFont.FromName (fontName, 30f);
				titleLabel.Font = UIFont.FromName (fontName, 25f);
			} else {
				daysUntilXmasLabel.Font = UIFont.FromName (fontName, 54f);
				titleLabel.Font = UIFont.FromName (fontName, 35f);
			}

			label = new UILabel (new RectangleF (0, 0, scrollViewWidth, scrollViewHeight));
			label2 = new UILabel (new RectangleF (scrollViewWidth, 0, scrollViewWidth, scrollViewHeight));
			label3 = new UILabel (new RectangleF (scrollViewWidth * 2, 0, scrollViewWidth, scrollViewHeight));
			label4 = new UILabel (new RectangleF (scrollViewWidth * 3, 0, scrollViewWidth, scrollViewHeight));

			label.Text = _time.DaysUntil;
			if (UserInterfaceIdiomIsPhone)
				label.Font = UIFont.FromName (fontName, 120f);
			else
				label.Font = UIFont.FromName (fontName, 200f);
			label.TextColor = UIColor.White;
			label.AutoresizingMask = UIViewAutoresizing.All;
			label.AdjustsFontSizeToFitWidth = true;
			label.Lines = 1;
			if (UserInterfaceIdiomIsPhone)
				label.MinimumFontSize = 20f;
			else
				label.MinimumFontSize = 100f;
			label.BackgroundColor = UIColor.Clear;
			scrollView.AddSubview (label);
			
			label2.Text = _time.HoursUntil;
			if (UserInterfaceIdiomIsPhone)
				label2.Font = UIFont.FromName (fontName, 120f);
			else
				label2.Font = UIFont.FromName (fontName, 200f);
			label2.TextColor = UIColor.White;
			label2.AutoresizingMask = UIViewAutoresizing.All;
			label2.AdjustsFontSizeToFitWidth = true;
			label2.Lines = 1;
			if (UserInterfaceIdiomIsPhone)
				label2.MinimumFontSize = 80f;
			else
				label2.MinimumFontSize = 100f;
			label2.BackgroundColor = UIColor.Clear;
			scrollView.AddSubview (label2);
			
			label3.Text = _time.MinutesUntil;
			if (UserInterfaceIdiomIsPhone)
				label3.Font = UIFont.FromName (fontName, 85f);
			else
				label3.Font = UIFont.FromName (fontName, 200f);
			label3.TextColor = UIColor.White;
			label3.AutoresizingMask = UIViewAutoresizing.All;
			label3.AdjustsFontSizeToFitWidth = true;
			label3.Lines = 1;
			if (UserInterfaceIdiomIsPhone)
				label3.MinimumFontSize = 100f;
			else
				label3.MinimumFontSize = 100f;
			label3.BackgroundColor = UIColor.Clear;
			scrollView.AddSubview (label3);
			
			label4.Text = _time.SecondsUntil;
			if (UserInterfaceIdiomIsPhone)
				label4.Font = UIFont.FromName (fontName, 60f);
			else
				label4.Font = UIFont.FromName (fontName, 150f);
			label4.TextColor = UIColor.White;
			label4.AutoresizingMask = UIViewAutoresizing.All;
			label4.AdjustsFontSizeToFitWidth = true;
			label4.Lines = 1;
			if (UserInterfaceIdiomIsPhone)
				label4.MinimumFontSize = 60f;
			else
				label4.MinimumFontSize = 100f;
			label4.BackgroundColor = UIColor.Clear;
			scrollView.AddSubview (label4);

			dayLabel.SetTitleColor (UIColor.White, UIControlState.Normal);
			dayLabel.Font = UIFont.FromName (fontName, 35f);
			dayLabel.TouchUpInside += (sender, e) => {
				scrollView.SetContentOffset (new PointF (0, 0), true);
				UpdatePageScroll (0);
			};
			
			hourLabel.Font = UIFont.FromName (fontName, 35f);
			hourLabel.TouchUpInside += (sender, e) => {
				scrollView.SetContentOffset (new PointF (scrollViewWidth, 0), true);
				UpdatePageScroll (1);
			};

			minuteLabel.Font = UIFont.FromName (fontName, 35f);
			minuteLabel.TouchUpInside += (sender, e) => {
				scrollView.SetContentOffset (new PointF (scrollViewWidth * 2, 0), true);
				UpdatePageScroll (2);
			};

			secondsLabel.Font = UIFont.FromName (fontName, 35f);
			secondsLabel.TouchUpInside += (sender, e) => {
				scrollView.SetContentOffset (new PointF (scrollViewWidth * 3, 0), true);
				UpdatePageScroll (3);
			};

			if (UserInterfaceIdiomIsPhone) {
				var scrollLabelsYPosition = UIScreen.MainScreen.Bounds.Height - scrollView.Bounds.Height + 20f;

				if (UIScreen.MainScreen.Bounds.Height > 480)
					scrollLabelsYPosition = UIScreen.MainScreen.Bounds.Height - scrollView.Bounds.Height - 50f;

				scrollLabels.Frame = new RectangleF (scrollLabels.Frame.X, scrollLabelsYPosition, scrollLabels.Frame.Width, scrollLabels.Frame.Height);
			}
		}

		void UpdatePageScroll (int? pageNum)
		{
			if (pageNum.HasValue)
				UpdatePageScroll (pageNum.Value, true);
			else 
				UpdatePageScroll (true);
		}

		void UpdatePageScroll (int pageNum, bool animated)
		{
			pageControl.CurrentPage = pageNum;

			switch(pageNum)
			{
			case 0:
				dayLabel.SetTitleColor(UIColor.White, UIControlState.Normal);
				hourLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				minuteLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				secondsLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				if (UserInterfaceIdiomIsPhone)
					scrollLabels.SetContentOffset(new Point(0, 0), animated);
				break;
			case 1:
				dayLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				hourLabel.SetTitleColor(UIColor.White, UIControlState.Normal);
				minuteLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				secondsLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				if (UserInterfaceIdiomIsPhone)
					scrollLabels.SetContentOffset(new Point((int)dayLabel.Bounds.Width + 5, 0), animated);
				break;
			case 2:
				dayLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				hourLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				minuteLabel.SetTitleColor(UIColor.White, UIControlState.Normal);
				secondsLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				if (UserInterfaceIdiomIsPhone)
					scrollLabels.SetContentOffset(new Point((int)dayLabel.Bounds.Width + (int)hourLabel.Bounds.Width + 5, 0), animated);
				break;
			case 3:
				dayLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				hourLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				minuteLabel.SetTitleColor(UIColor.Black, UIControlState.Normal);
				secondsLabel.SetTitleColor(UIColor.White, UIControlState.Normal);
				if (UserInterfaceIdiomIsPhone)
					scrollLabels.SetContentOffset(new Point((int)dayLabel.Bounds.Width + (int)hourLabel.Bounds.Width + (int)minuteLabel.Bounds.Width + 12, 0), animated);

				break;
			}

		}


		void UpdatePageScroll (bool animated)
		{
			UpdatePageScroll(GetPageNumber (), animated);
		}

		int GetPageNumber(){
			return (int)(scrollView.ContentOffset.X / scrollView.Frame.Width);
		}

		public void AddNightFallGradient ()
		{
			CAGradientLayer gradient =  CAGradientLayer.Create() as CAGradientLayer;
			gradient.Frame = View.Bounds;
			gradient.Colors = new MonoTouch.CoreGraphics.CGColor[] { UIColor.FromRGB(0, 0, 0).CGColor, UIColor.FromRGB(0, 71, 93).CGColor }; 
			View.Layer.InsertSublayer(gradient, 0);
		}

		void AddButtons ()
		{
			settingsButton.TouchUpInside += (object sender, EventArgs e) => {
				if(UserInterfaceIdiomIsPhone)
				{
					actionSheet = new UIActionSheet("Settings", null, "Cancel", null, new [] { "Music", "About"});
					actionSheet.Style = UIActionSheetStyle.BlackTranslucent;
					actionSheet.Clicked += (object sheetSender, UIButtonEventArgs eve) => {
						switch(eve.ButtonIndex)
						{
						case 0:
							DisplayMusicOptions();
							break;
						case 1:
							DisplaySettingsOptions();
							break;
						case 2:
							break;
						}
					};
					actionSheet.ShowFrom(musicButton.Frame, View, true);
				} else {
					DisplaySettingsOptions();
				}
			};

			musicButton.TouchUpInside += (object sender, EventArgs e) => {
				DisplayMusicOptions();
			};
		}

		void DisplaySettingsOptions ()
		{
			if (UserInterfaceIdiomIsPhone) {
				aboutViewController = new AboutViewController (this);
				aboutViewController.Done += (button, even) => {
					DismissViewController (true, null);
				};
				PresentViewController (aboutViewController, true, null);
			} else {
				if (aboutViewController == null) {
					var controller = new AboutViewController (this);
					aboutPopoverController = new UIPopoverController (controller);
					aboutPopoverController.PopoverContentSize = new SizeF (320f, 420f);
					controller.Done += delegate {
						aboutPopoverController.Dismiss (true);
					};
				}
			
				if (aboutPopoverController.PopoverVisible) {
					aboutPopoverController.Dismiss (true);
				} else {
					aboutPopoverController.PresentFromRect (settingsButton.Frame, View, UIPopoverArrowDirection.Any, true);
				}
			}
		}

		void DisplayMusicOptions ()
		{
			if (UserInterfaceIdiomIsPhone) {
				flipsideViewController = new FlipsideViewController (this);
				flipsideViewController.Done += (button, even) => {
					DismissViewController (true, null);
				};
				PresentViewController (flipsideViewController, true, null);
			} else {
				if (flipsidePopoverController == null) {
					var controller = new FlipsideViewController (this);
					flipsidePopoverController = new UIPopoverController (controller);
					flipsidePopoverController.PopoverContentSize = new SizeF (320f, 460f);
					controller.Done += delegate {
						flipsidePopoverController.Dismiss (true);
					};
				}
				
				if (flipsidePopoverController.PopoverVisible) {
					flipsidePopoverController.Dismiss (true);
				} else {
					flipsidePopoverController.PresentFromRect (musicButton.Frame, View, UIPopoverArrowDirection.Any, true);
				}
			}
		}

		void PopulateTimeInformation ()
		{
			var timeDifference = TimeHelper.GetTimeDifference ();
			
			_time.DaysUntil = String.Format ("{0:0,0}", (int)timeDifference.TotalDays + 1);

			if (DateTime.Now.Day == 25 && DateTime.Now.Month == 12) {
				_time.DaysUntil = "Today";
				_time.SecondsUntil = "Today";
				_time.MinutesUntil = "Today";
				_time.HoursUntil = "Today";
			} else {
				_time.SecondsUntil = String.Format("{0:0,0}", (int)timeDifference.TotalSeconds);
				_time.MinutesUntil = String.Format("{0:0,0}", (int)timeDifference.TotalMinutes);
				_time.HoursUntil = String.Format("{0:0,0}", (int)timeDifference.TotalHours);
			}
			daysUntilXmasLabel.Text = _time.DaysUntil;

		}

		public string GetSocialCountdownString()
		{
			var pageNumber = GetPageNumber ();
			string time, unit = "";
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
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return true;
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
	}
}

