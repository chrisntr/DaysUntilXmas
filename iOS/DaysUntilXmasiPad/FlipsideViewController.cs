using System;
using System.Drawing;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoTouch.CoreAnimation;

namespace DaysUntilXmasiPad
{
	public partial class FlipsideViewController : UIViewController
	{

		MainViewController mainViewController;


		public FlipsideViewController (MainViewController mvc) : base ("FlipsideViewController", null)
		{
			mainViewController = mvc;
		}

		UIColor highlightedColor = UIColor.FromRGB(0, 71, 93);
		UIColor normalColor = UIColor.Gray;

		public override void ViewWillAppear (bool animated)
		{
			if (!MainViewController.UserInterfaceIdiomIsPhone) {
				this.View.Bounds = new RectangleF (View.Frame.X, View.Frame.Y, 320f, 460f);
			}
			base.ViewWillAppear (animated);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var fontName = "Open Sans";

			View.Subviews.OfType<UIButton>().Each( (button, n) => {
				button.TitleLabel.Font = UIFont.FromName (fontName, 22f);
				// First five buttons are the title tracks
				if(n < 5){
					SetHighlightedTrack(button, n);
					mainViewController.musicOption.MusicItems[n].PropertyChanged += (sender, ev) => {
						SetHighlightedTrack(button, n);
					};
					button.TouchUpInside += (send, ea) => {
						mainViewController.PlayAudio(mainViewController.musicOption.MusicItems[n].Path);
						mainViewController.musicOption.SetDefaultMusic(n);
					};
				}
			});

			CAGradientLayer gradient =  CAGradientLayer.Create() as CAGradientLayer;
			gradient.Frame = View.Bounds;
			gradient.Colors = new MonoTouch.CoreGraphics.CGColor[] { UIColor.FromRGB(0, 0, 0).CGColor, UIColor.FromRGB(0, 71, 93).CGColor }; 
			View.Layer.InsertSublayer(gradient, 0);

			muteButton.TitleLabel.Font = UIFont.FromName (fontName, 22f);
			musicLabel.Font = UIFont.FromName (fontName, 16f);
			muteSwitch.On = SettingsHelper.Mute;
			muteSwitch.ValueChanged += (object sender, EventArgs e) => {
				mainViewController.MuteAudio(muteSwitch.On);
			};
			muteButton.SetTitleColor(highlightedColor, UIControlState.Highlighted);
			muteButton.TouchUpInside += (sender, e) => {
				muteSwitch.On = !muteSwitch.On;
				mainViewController.MuteAudio(muteSwitch.On);
			};
		}

		void SetHighlightedTrack (UIButton b, int i)
		{
			b.SetTitleColor(highlightedColor, UIControlState.Highlighted);
			if(mainViewController.musicOption.MusicItems[i].DefaultOption)
				b.SetTitleColor(highlightedColor, UIControlState.Normal);
			else
				b.SetTitleColor(normalColor, UIControlState.Normal);
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
		
		partial void done (UIBarButtonItem sender)
		{
			if (Done != null)
				Done (this, EventArgs.Empty);
		}
		
		public event EventHandler Done;
	}
}

