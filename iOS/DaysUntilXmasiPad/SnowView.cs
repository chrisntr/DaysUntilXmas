using System;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using System.Drawing;
using MonoTouch.Foundation;

namespace DaysUntilXmasiPad
{
	[Register("SnowView")]
	public class SnowView : UIView
	{

		public CAEmitterLayer emitter;

		public SnowView () : base()
		{

			Initiate();
		}
		
		public SnowView (RectangleF frame) : base(frame) 
		{
			Initiate();
		}
		
		public SnowView (IntPtr handle) : base(handle) 
		{
			Initiate();
		}

		public SnowView (NSObjectFlag t) : base(t) 
		{
			Initiate();
		}

		public SnowView (NSCoder coder) : base(coder) 
		{
			Initiate();
		}

		public void Initiate ()
		{
			SetUpSnowField();
		}

		public void SetUpSnowField()
		{
			
			emitter = new CAEmitterLayer();
			this.BackgroundColor = UIColor.Clear;
			emitter.Position = new PointF(UIScreen.MainScreen.Bounds.Width /2, -10f);
			emitter.Size = new SizeF(UIScreen.MainScreen.Bounds.Width,1);
			emitter.Shape = CAEmitterLayer.ShapeLine;

			var cell = new CAEmitterCell();
			cell.BirthRate = 10f;
			cell.LifeTime = 9.0f;
			cell.Contents = UIImage.FromFile("snow-1.png").CGImage;
			cell.Velocity = 10f;
			cell.VelocityRange = 50f;
			cell.EmissionRange = (float) (2f*Math.PI);
			cell.EmissionLongitude = (float) Math.PI;
			cell.AccelerationY = 40f;
			cell.Scale = 1.0f;
			cell.ScaleRange = 0.2f;
			cell.SpinRange = 10.0f;

			emitter.Cells = new CAEmitterCell[] { cell };//, cell2, cell3 };
			emitter.RenderMode = CAEmitterLayer.RenderUnordered; 
			
			Layer.AddSublayer(emitter);
		}
	}
}

