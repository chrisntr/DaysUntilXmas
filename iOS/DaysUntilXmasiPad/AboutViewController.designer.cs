// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace DaysUntilXmasiPad
{
	[Register ("AboutViewController")]
	partial class AboutViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel aboutDescription { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel websiteDescription { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton shareButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton websiteLink { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton socialLink { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton twitterLink { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton reviewLink { get; set; }

		[Action ("done:")]
		partial void done (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (aboutDescription != null) {
				aboutDescription.Dispose ();
				aboutDescription = null;
			}

			if (websiteDescription != null) {
				websiteDescription.Dispose ();
				websiteDescription = null;
			}

			if (shareButton != null) {
				shareButton.Dispose ();
				shareButton = null;
			}

			if (websiteLink != null) {
				websiteLink.Dispose ();
				websiteLink = null;
			}

			if (socialLink != null) {
				socialLink.Dispose ();
				socialLink = null;
			}

			if (twitterLink != null) {
				twitterLink.Dispose ();
				twitterLink = null;
			}

			if (reviewLink != null) {
				reviewLink.Dispose ();
				reviewLink = null;
			}
		}
	}
}
