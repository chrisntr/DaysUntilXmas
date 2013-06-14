// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace DaysUntilXmasiPad
{
	[Register ("FlipsideViewController")]
	partial class FlipsideViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton song1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton song2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton song3 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton song4 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton song5 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch muteSwitch { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton muteButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel musicLabel { get; set; }

		[Action ("done:")]
		partial void done (MonoTouch.UIKit.UIBarButtonItem sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (song1 != null) {
				song1.Dispose ();
				song1 = null;
			}

			if (song2 != null) {
				song2.Dispose ();
				song2 = null;
			}

			if (song3 != null) {
				song3.Dispose ();
				song3 = null;
			}

			if (song4 != null) {
				song4.Dispose ();
				song4 = null;
			}

			if (song5 != null) {
				song5.Dispose ();
				song5 = null;
			}

			if (muteSwitch != null) {
				muteSwitch.Dispose ();
				muteSwitch = null;
			}

			if (muteButton != null) {
				muteButton.Dispose ();
				muteButton = null;
			}

			if (musicLabel != null) {
				musicLabel.Dispose ();
				musicLabel = null;
			}
		}
	}
}
