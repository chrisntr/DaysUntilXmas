// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace DaysUntilXmasiPad
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton settingsButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton musicButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton dayLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton hourLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton minuteLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton secondsLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel daysUntilXmasLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel titleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView scrollLabels { get; set; }

		[Action ("showInfo:")]
		partial void showInfo (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (settingsButton != null) {
				settingsButton.Dispose ();
				settingsButton = null;
			}

			if (musicButton != null) {
				musicButton.Dispose ();
				musicButton = null;
			}

			if (dayLabel != null) {
				dayLabel.Dispose ();
				dayLabel = null;
			}

			if (hourLabel != null) {
				hourLabel.Dispose ();
				hourLabel = null;
			}

			if (minuteLabel != null) {
				minuteLabel.Dispose ();
				minuteLabel = null;
			}

			if (secondsLabel != null) {
				secondsLabel.Dispose ();
				secondsLabel = null;
			}

			if (daysUntilXmasLabel != null) {
				daysUntilXmasLabel.Dispose ();
				daysUntilXmasLabel = null;
			}

			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (scrollLabels != null) {
				scrollLabels.Dispose ();
				scrollLabels = null;
			}
		}
	}
}
