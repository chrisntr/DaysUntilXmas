using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Shell;

namespace TimeLibrary
{
    public static class UpdateTile
    {
        public static void Start()
        {
            Debug.WriteLine("Starting to update title...");
            var timeSpan = TimeHelper.GetTimeDifference();
            Uri imageUrl;

            Debug.WriteLine("Total days until Xmas = " + timeSpan.TotalDays);

            if(DateTime.Now.Day == 25 && DateTime.Now.Month == 12)
            {
                imageUrl = new Uri("/Images/00.png", UriKind.Relative);
            } 
            else if (timeSpan.TotalDays > 99)
            {
                Debug.WriteLine("Showing \"default title\"");
                // Display "main" tile
                imageUrl = new Uri("/Background.png", UriKind.Relative);
            }
            else
            {
                // It's near Christmas so lets update the live tile

                Debug.WriteLine("Showing Live tile because Xmas is coming...");
                var daysLeft = String.Format("{0:0,0}", (int)timeSpan.TotalDays + 1);
                imageUrl = new Uri("/Images/" + daysLeft + ".png", UriKind.Relative);
            }


            Debug.WriteLine("Image URI to be set = "+ imageUrl);

            var shellData = new StandardTileData
            {
                BackgroundImage = imageUrl,
                BackBackgroundImage = null
            };

            ShellTile.ActiveTiles.First().Update(shellData);
        }
    }
}
