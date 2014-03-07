using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Views.Animations;
using Android.Graphics;

namespace DaysUntilXmasAndroid
{
	public class ProxyDrawable : Drawable
	{
		private Drawable mProxy;

		public ProxyDrawable(Drawable target) {
			mProxy = target;
		}

		public Drawable Proxy {
			get {
				return mProxy;
			}
			set {
				if (value != this) {
					mProxy = value;
				}
			}
		}

		public override void Draw (Android.Graphics.Canvas canvas)
		{
			if (mProxy != null) {
				mProxy.Draw(canvas);
			}
		}

		public override int IntrinsicWidth {
			get {
				return mProxy != null ? mProxy.IntrinsicWidth : -1;
			}
		}

		public override int IntrinsicHeight {
			get {
				return mProxy != null ? mProxy.IntrinsicHeight : -1;
			}
		}

		public override int Opacity {
			get {
				return mProxy != null ? mProxy.Opacity : (int) Format.Transparent;
			}
		}

		public override void SetFilterBitmap (bool filter)
		{
			if (mProxy != null) {
				mProxy.SetFilterBitmap(filter);
			}
		}

		public override void SetDither (bool dither)
		{
			if (mProxy != null) {
				mProxy.SetDither(dither);
			}
		}

		public override void SetColorFilter (ColorFilter cf)
		{
			if (mProxy != null) {
				mProxy.SetColorFilter(cf);
			}
		}

		public override void SetAlpha (int alpha)
		{
			if (mProxy != null) {
				mProxy.SetAlpha(alpha);
			}
		}
	}
}


