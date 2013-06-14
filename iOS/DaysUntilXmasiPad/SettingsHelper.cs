using System;
using MonoTouch.Foundation;

namespace DaysUntilXmasiPad
{
	public static class SettingsHelper
	{
		// Do the same but with OAuthTokenSecret here.
		public static bool Mute {
			get {
				return NSUserDefaults.StandardUserDefaults.BoolForKey ("Mute");
			}
			set {
				NSUserDefaults.StandardUserDefaults.SetBool (value, "Mute");
				NSUserDefaults.StandardUserDefaults.Synchronize ();
			}
		}

		public static string SelectedSong {
			get {
				if (!String.IsNullOrEmpty (NSUserDefaults.StandardUserDefaults.StringForKey ("SelectedSong")))
					return NSUserDefaults.StandardUserDefaults.StringForKey ("SelectedSong");
				else 
					return null;
			}
			set {
				NSUserDefaults.StandardUserDefaults.SetString (value, "SelectedSong");
				NSUserDefaults.StandardUserDefaults.Synchronize ();
			}
		}
	}
}

