using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DaysUntilChristmas
{
	public partial class SnowFlake : UserControl
	{
        static private Random randomNumber = new Random();

        private double x = 0;
        private double y = 0;

        private double xSpeed = 0;
        private double ySpeed = 0;

        private double radius = 0;

        private double scale = 0;
        private double alpha = 0;

        //
        // Responsible for setting the boundaries
        //
        private Point StageSize
        {
            get;
            set;
        }

		public SnowFlake(ImageSource imageSource)
		{
			// Required to initialize variables
			InitializeComponent();

            ImageIcon.Source = imageSource;

            // We don't want this to run on the design surface!
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false)
            {
                //SetInitialProperties();
            }
		}

        //
        // All of the various properties that you want to set initially is done here
        //
        public void SetInitialProperties(int stageWidth, int stageHeight)
        {
            //Setting the various parameters that need tweaking
            xSpeed = randomNumber.NextDouble()/20;
            ySpeed = .01 + randomNumber.NextDouble() * 2;
            radius = randomNumber.NextDouble();
            scale = .01 + randomNumber.NextDouble() * 2;
            alpha = .1 + randomNumber.NextDouble();

            //Setting initial position
            Canvas.SetLeft(this, randomNumber.Next(stageWidth));
            Canvas.SetTop(this, 0 - 10);
            //Canvas.SetTop(this, randomNumber.Next(stageHeight));

            StageSize = new Point(stageWidth, stageHeight);

            y = Canvas.GetTop(this);

            //Setting initial size and opacity
            this.Width = 5 * scale;
            this.Height = 5 * scale;
            this.Opacity = alpha;

            //Starting our animation loop
            CompositionTarget.Rendering += new EventHandler(MoveSnowFlake);
        }

        //
        // This method gets called many times a second and is responsible for moving your snowflake around
        //
        void MoveSnowFlake(object sender, EventArgs e)
        {
            x += xSpeed;
            y += ySpeed;

            Canvas.SetTop(this, y);
            Canvas.SetLeft(this, Canvas.GetLeft(this) + radius*Math.Cos(x));

            // Reset the position to go back to the top when the bottom boundary is reached
            if (Canvas.GetTop(this) > StageSize.Y)
            {
                Canvas.SetTop(this, -this.ActualHeight-10);
                y = Canvas.GetTop(this);
            }
        }
	}
}