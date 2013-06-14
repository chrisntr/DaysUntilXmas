
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Twitter;
using MonoTouch.CoreAnimation;
using System.Linq;

namespace DaysUntilXmasiPad
{
	public partial class AboutViewController : UIViewController
	{
		MainViewController mainViewController;

		public AboutViewController (MainViewController mvc) : base ("AboutViewController", null)
		{
			mainViewController = mvc;

		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear (bool animated)
		{
			if (!MainViewController.UserInterfaceIdiomIsPhone) {
				this.View.Bounds = new RectangleF (View.Frame.X, View.Frame.Y, 320f, 420f);
			}
			base.ViewWillAppear (animated);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.Subviews.OfType<UIButton>().Each( (button, n) => {
				button.TitleLabel.Font = UIFont.FromName ("Open Sans", 18f);
			});

			View.Subviews.OfType<UILabel>().Each( (label, n) => {
				label.Font = UIFont.FromName ("Open Sans", 16f);
			});

			CAGradientLayer gradient =  CAGradientLayer.Create() as CAGradientLayer;
			gradient.Frame = View.Bounds;
			gradient.Colors = new MonoTouch.CoreGraphics.CGColor[] { UIColor.FromRGB(0, 0, 0).CGColor, UIColor.FromRGB(0, 71, 93).CGColor }; 
			View.Layer.InsertSublayer(gradient, 0);

			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
				twitterLink.Hidden = true;
				socialLink.Hidden = false;
			} else {
				twitterLink.Hidden = false;
				socialLink.Hidden = true;
			}

			twitterLink.TouchUpInside += (sender, e) => {
				var tweet = new TWTweetComposeViewController();
				tweet.SetInitialText (mainViewController.GetSocialCountdownString());
				PresentViewController(tweet, true, null);
			};

			socialLink.TouchUpInside += (sender, e) => {
				var message = mainViewController.GetSocialCountdownString();
				var social = new UIActivityViewController(new NSObject[] { new NSString(message)}, 
				new UIActivity[] { new UIActivity() });
				PresentViewController(social, true, null);
			};

			websiteLink.TouchUpInside += (sender, e) => {
				var webUrl = NSUrl.FromString("http://daysuntilxmas.com");
				UIApplication.SharedApplication.OpenUrl(webUrl);
			};

			reviewLink.TouchUpInside += (sender, e) => {
				var id = "yourAppId";
				var itunesUrl = NSUrl.FromString(String.Format("itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id={0}", id));
				var webUrl = NSUrl.FromString(String.Format("http://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id={0}&pageNumber=0&sortOrdering=1&type=Purple+Software&mt=8", id));
				if(UIApplication.SharedApplication.CanOpenUrl(itunesUrl))
					UIApplication.SharedApplication.OpenUrl(itunesUrl);
				else
					UIApplication.SharedApplication.OpenUrl(webUrl);
			};

			// Perform any additional setup after loading the view, typically from a nib.
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
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		partial void done (NSObject sender)
		{
			if (Done != null)
				Done (this, EventArgs.Empty);

		}
		
		public event EventHandler Done;
	}
}

