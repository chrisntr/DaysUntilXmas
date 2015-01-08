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
using Android.Support.V4.View;

namespace DaysUntilXmasAndroid
{
	public class SnowFallView : View
	{
		#if WEAR
		private int snow_flake_count = 10;
		#else
		private int snow_flake_count = 50;
		#endif
		private List<Drawable> drawables = new List<Drawable>();
		private int[][] coords;
		private Drawable snow_flake, snow_flake2, snow_flake3, snow_flake4;
		private Random random;


		public SnowFallView(Context context, Android.Util.IAttributeSet attrs) : base(context, attrs)
		{
			Init (context);
		}

		public SnowFallView(Context context) : base(context)
		{
			Init (context);
		}

		private void Init(Context context)
		{
			random = new Random ();
			Focusable = true;
			FocusableInTouchMode = true;

			snow_flake = context.Resources.GetDrawable (Resource.Drawable.snow_1);
			snow_flake.Bounds = new Android.Graphics.Rect (0, 0, 9, 10); // snow_flake.IntrinsicWidth, snow_flake.IntrinsicHeight);
			snow_flake2 = context.Resources.GetDrawable (Resource.Drawable.snow_2);
			snow_flake2.Bounds = new Android.Graphics.Rect (0, 0, 10, 9); // snow_flake.IntrinsicWidth, snow_flake.IntrinsicHeight);
			snow_flake3 = context.Resources.GetDrawable (Resource.Drawable.snow_3);
			snow_flake3.Bounds = new Android.Graphics.Rect (0, 0, 9, 10); // snow_flake.IntrinsicWidth, snow_flake.IntrinsicHeight);
			snow_flake4 = context.Resources.GetDrawable (Resource.Drawable.snow_4);
			snow_flake4.Bounds = new Android.Graphics.Rect (0, 0, 10, 9); // snow_flake.IntrinsicWidth, snow_flake.IntrinsicHeight);

		}

		protected override void OnSizeChanged (int width, int height, int oldw, int oldh)
		{
			base.OnSizeChanged (width, height, oldw, oldh);

			Random random = new Random();
			IInterpolator interpolator = new LinearInterpolator();

			coords = new int[snow_flake_count][];
			drawables.Clear();
			for (int i = 0; i < snow_flake_count; i++) {
				//Console.WriteLine ("Add animation for number " + i);
				Animation animation = new TranslateAnimation(0, height / 10 - random.Next(height / 5), 0, height + 50);
				animation.Duration = (10 * height + random.Next(10 * height)) + 3000;
				animation.RepeatCount = -1;
				animation.Initialize(10, 10, 10, 10);
				animation.Interpolator = interpolator;

				coords[i] = new int[] { random.Next(width - 30), - 40 };
				//Console.WriteLine ("Coords = " + coords[i][0] + " and " + coords[i][1]);
				var snow = GetSnowFlake ();
				snow.SetAlpha (random.Next (180, 255));
				drawables.Add(new AnimateDrawable(snow, animation));
				animation.StartOffset = random.Next(20 * height);
				animation.StartNow();
			}
		}

		private Drawable GetSnowFlake()
		{
			var nextRand = random.Next (0, 100);

			if (nextRand > 75)
				return snow_flake4;

			if (nextRand > 50)
				return snow_flake3;

			if (nextRand > 25)
				return snow_flake2;

			return snow_flake;
		}

		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
			base.OnDraw (canvas);
			for (int i = 0; i < snow_flake_count; i++) {
				using (var drawable = drawables[i]) {
					canvas.Save ();
					canvas.Translate (coords[i][0], coords [i] [1]);
					drawable.Draw (canvas);
					canvas.Restore ();
				}
			}
			Invalidate();

		}
	}
}


