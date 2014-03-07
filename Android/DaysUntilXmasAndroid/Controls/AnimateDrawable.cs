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

namespace DaysUntilXmasAndroid
{
	public class AnimateDrawable : ProxyDrawable
	{

		private Animation mAnimation;
		private Transformation mTransformation = new Transformation();

		public AnimateDrawable(Drawable target) : base(target) 
		{
		}

		public AnimateDrawable(Drawable target, Animation animation) : base(target)
		{
			mAnimation = animation;
		}

		public Animation Animation {
			get {
				return mAnimation;
			}
			set {
				mAnimation = value;
			}
		}

		public bool HasStarted {
			get {
				return mAnimation != null && mAnimation.HasStarted;
			}
		}

		public bool HasEnded {
			get {
				return mAnimation == null || mAnimation.HasEnded;
			}
		}

		public override void Draw (Android.Graphics.Canvas canvas)
		{

			Drawable dr = Proxy;
			if (dr != null) {
				int sc = canvas.Save();
				Animation anim = mAnimation;
				if (anim != null) {
					anim.GetTransformation(
						AnimationUtils.CurrentAnimationTimeMillis(),
						mTransformation);
					canvas.Concat(mTransformation.Matrix);
				}
				dr.Draw(canvas);
				canvas.RestoreToCount(sc);
			}
		}
	}
}


