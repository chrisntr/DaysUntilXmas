using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using TimeLibrary;

namespace DaysUntilChristmas
{
    public partial class MainPage : PhoneApplicationPage
    {
        const int MaxSnowFlakes = 100;
        int _currentSnowFlakes = 0;
        static readonly Random RandomNumber = new Random();
        MusicOptions musicOption = new MusicOptions();

        BitmapImage _snow1;
        BitmapImage _snow2;
        BitmapImage _snow3;
        BitmapImage _snow4;

        BitmapImage[] _snowChoices;

        readonly DispatcherTimer _dayTimer = new DispatcherTimer();
        readonly DispatcherTimer _snowTimer = new DispatcherTimer();
        readonly TimeInformation _time = new TimeInformation();
        
        private bool _aboutScreenVisible, _musicScreenVisible = false;

        PeriodicTask periodicTask;
        string periodicTaskName = "LiveTileTask";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildApplicationBar();

            _dayTimer.Interval = TimeSpan.FromSeconds(1);
            _dayTimer.Tick += DispatcherTimerTick;
            _dayTimer.Start();

            PopulateTimeInformation();

            hoursUntil.FontSize = GetFontSize(_time.HoursUntil.Length);

            PageTitle.DataContext = _time;
            pivotControl.DataContext = _time;

            SetUpMusic();

            SetUpMusicOptionsPage();

            SetUpAboutPageButtons();

            SetUpNotifications();

            PopulateSnowFlakes();
        }

        private void SetUpNotifications()
        {

            // Update live title anyway:
            UpdateTile.Start();

            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

            // If the task already exists and background agents are enabled for the
            // application, you must remove the task and then add it again to update 
            // the schedule
            if (periodicTask != null)
            {
                RemoveAgent(periodicTaskName);
            }

            try
            {
                periodicTask = new PeriodicTask(periodicTaskName);
                // The description is required for periodic agents. This is the string that the user
                // will see in the background services Settings page on the device.
                periodicTask.Description = "This is the Live Tile support for the Days Until Xmas application. Live tiles will not work without this running.";

                ScheduledActionService.Add(periodicTask);
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show(
                        "Background agents for this application have been disabled by the user. Please enable for Live Tiles.",
                        "Whoops!", MessageBoxButton.OK);
                }
            }

        }

        private void RemoveAgent(string name)
        {
            try
            {
                ScheduledActionService.Remove(name);
            }
            catch (Exception)
            {
            }
        }

        // Helper function to build a localized ApplicationBar
        private void BuildApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources
            var appBarButton = new ApplicationBarIconButton(new Uri("Images/settings.png", UriKind.Relative));
            appBarButton.Text = AppResources.About;
            appBarButton.Click += AboutButtonClicked;
            ApplicationBar.Buttons.Add(appBarButton);


            var appBarButton2 = new ApplicationBarIconButton(new Uri("Images/music.png", UriKind.Relative));
            appBarButton2.Text = AppResources.Music;
            appBarButton2.Click += MusicButtonClicked;
            ApplicationBar.Buttons.Add(appBarButton2);

            ApplicationBar.ForegroundColor = Colors.White;
            ApplicationBar.Opacity = 0;

        }

        private void SetUpAboutPageButtons()
        {
            const string fallingSnowUrl = "http://blogs.msdn.com/b/expression/archive/2008/12/27/creating-falling-snow-in-silverlight.aspx";
            fallingSnowLink.Click += (s, e) =>
            {
                var task = new WebBrowserTask {Uri = new Uri(fallingSnowUrl)};
                task.Show();  
            };

            reviewButton.Click += (s, e) =>
            {
                AboutFlipBack.Begin();
                var reviewTask = new MarketplaceReviewTask();
                reviewTask.Show();
            };

            sendToFriendButton.Click += (s, e) =>
            {
                const string marketplaceUrl = "http://bit.ly/acLe1V"; 
                var emailTask = new EmailComposeTask();
                emailTask.Subject = "Days Until Christmas on Windows Phone 7!";
                var sb = new StringBuilder();
                sb.Append(String.Format("Hey!\nCheck out this days until Christmas app on the Windows Phone 7 marketplace! Don't worry, it's free. \nDownload here: {0}\n", marketplaceUrl));
                sb.Append(String.Format("Only {0} {1} left until Christmas!\n", _time.DaysUntil.ToString(),
                    (_time.DaysUntil == "01") ? "day": "days"));
                sb.Append("Merry Christmas\n");
                emailTask.Body = sb.ToString();
                emailTask.Show();
            };

        }

        private void SetUpMusicOptionsPage()
        {
            SongList.ItemsSource = musicOption.MusicItems;
            SongList.SelectionChanged += (s, sce) =>
            {
                musicOption.SetDefaultMusic(SongList.SelectedIndex);
                PlayAudio();
            };
        }

        private void SetUpMusic()
        {

            var mute = String.Empty;
            if(IsolatedStorageSettings.ApplicationSettings.Contains("mute"))
                mute = IsolatedStorageSettings.ApplicationSettings["mute"].ToString();
            else
                IsolatedStorageSettings.ApplicationSettings.Add("mute", "false");
    
            audioPlayer.MediaEnded += delegate
            {
                Debug.WriteLine("Media Ended");
                audioPlayer.Position = TimeSpan.Zero;
                audioPlayer.Play(); 
            };

            audioPlayer.MediaFailed += (s, e) =>
            {
                Debug.WriteLine("media failed?" + e.ErrorException.Message);
            };

            if(!String.IsNullOrEmpty(mute) && mute == "true")
            {

                muteBox.Content = AppResources.On;
                muteBox.IsChecked = true;
                audioPlayer.IsMuted = true;
            }
            else
            {
                muteBox.Content = AppResources.Off;
                muteBox.IsChecked = false;
                audioPlayer.IsMuted = false;
            }

            if (!IsAudioAlreadyPlaying() && mute != "true")
            {
                PlayAudio();
            }
            else
                muteBox.IsChecked = true;
            
            muteBox.Checked += (s, e) =>
            {
                muteBox.Content = AppResources.On;
                IsolatedStorageSettings.ApplicationSettings["mute"] = "true";
                IsolatedStorageSettings.ApplicationSettings.Save();
                audioPlayer.IsMuted = true;
            };
            muteBox.Unchecked += (s, e) =>
            {
                muteBox.Content = AppResources.Off;
                try
                {
                    IsolatedStorageSettings.ApplicationSettings["mute"] = "false";
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
                catch(IsolatedStorageException er)
                {
                    throw new Exception(er.Message);
                }

                if (!IsAudioAlreadyPlaying())
                {
                    PlayAudio();
                }
                audioPlayer.IsMuted = false;
            };
        }

        private static bool IsAudioAlreadyPlaying()
        {
            bool result;
            try
            {
                result = MediaPlayer.State == MediaState.Playing || BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing;

            } catch
            {
                result = false;
            }
            return result;
        }

        private void PlayAudio()
        {
            audioPlayer.Stop();
            audioPlayer.Source = new Uri(musicOption.MusicItems.FirstOrDefault(x => x.DefaultOption).Path, UriKind.Relative);
            audioPlayer.IsMuted = false;
            audioPlayer.Volume = 1;
            audioPlayer.Play();
        }

        private void PopulateSnowFlakes()
        {
            _snow1 = new BitmapImage(new Uri("/Images/snow-1.png", UriKind.Relative));
            _snow2 = new BitmapImage(new Uri("/Images/snow-2.png", UriKind.Relative));
            _snow3 = new BitmapImage(new Uri("/Images/snow-3.png", UriKind.Relative));
            _snow4 = new BitmapImage(new Uri("/Images/snow-4.png", UriKind.Relative));

            _snowChoices = new[] { _snow1, _snow2, _snow3, _snow4 };
            _snowTimer.Interval = TimeSpan.FromMilliseconds(80);
            _snowTimer.Tick += SnowTimerTick;
            _snowTimer.Start();
        }

        void SnowTimerTick(object sender, EventArgs e)
        {
            _currentSnowFlakes++;
            if(_currentSnowFlakes >= MaxSnowFlakes)
                _snowTimer.Stop();

            // Randomise which image gets used.
            var randomSnowFlake = RandomNumber.Next(_snowChoices.Count());
            var snowFlake = new SnowFlake(_snowChoices[randomSnowFlake]);

            // Set height and width of phone
            snowFlake.SetInitialProperties(400, 800);
            SnowField.Children.Add(snowFlake);
        }

        void DispatcherTimerTick(object sender, EventArgs e)
        {
            PopulateTimeInformation();
        }

        private void PopulateTimeInformation()
        {
            
            var timeDifference = TimeHelper.GetTimeDifference();

            _time.DaysUntil = String.Format("{0:0,0}", (int)timeDifference.TotalDays + 1);
            if (DateTime.Now.Day == 25 && DateTime.Now.Month == 12)
            {
                _time.DaysUntil = AppResources.Today;
                _time.SecondsUntil = AppResources.Today;
                _time.MinutesUntil = AppResources.Today;
                _time.HoursUntil = AppResources.Today;
            }
            else
            {
                _time.SecondsUntil = String.Format("{0:0,0}", (int)timeDifference.TotalSeconds);
                _time.MinutesUntil = String.Format("{0:0,0}", (int)timeDifference.TotalMinutes);
                _time.HoursUntil = String.Format("{0:0,0}", (int)timeDifference.TotalHours);
            }

            CalculateFontSizeForTime();
        }

        private void CalculateFontSizeForTime()
        {
            secondUntil.FontSize = GetFontSize(secondUntil.Text.Length);
            minutesUntil.FontSize = GetFontSize(minutesUntil.Text.Length);
            hoursUntil.FontSize = GetFontSize(hoursUntil.Text.Length);
            daysUntil.FontSize = GetFontSize(daysUntil.Text.Length);
        }

        /// <summary>
        /// Since Silverlight doesn't automatically set the size of the 
        /// text in a textblock - we have to manually adjust it.
        /// </summary>
        /// <param name="textLength"></param>
        /// <returns></returns>
        private static double GetFontSize(int textLength)
        {
            if (textLength == 10)
                return 96;
            if (textLength == 9)
                return 105;
            if (textLength == 8)
                return 120;
            if (textLength == 7)
                return 130;
            if (textLength == 6)
                return 150;
            if (textLength == 5)
                return 180;
            if (textLength == 4)
                return 225;
            if (textLength <= 3)
                return 250;
            return 100;

        }

        private void AboutButtonClicked(object sender, EventArgs e)
        {
            if(!_aboutScreenVisible)
            {
                AboutFlip.Begin();
                var musicButton = (ApplicationBarIconButton) ApplicationBar.Buttons[1];
                musicButton.IsEnabled = false;
            }
            else
            {
                AboutFlipBack.Begin();
                var musicButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
                musicButton.IsEnabled = true;
            }
  
            _aboutScreenVisible = !_aboutScreenVisible;
        }

        private void MusicButtonClicked(object sender, EventArgs e)
        {
            if (!_musicScreenVisible)
            {
                MusicFlip.Begin();
                var aboutButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                aboutButton.IsEnabled = false;
            }
            else
            {
                MusicFlipBack.Begin();
                var aboutButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                aboutButton.IsEnabled = true;
            }
            _musicScreenVisible = !_musicScreenVisible;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if(_aboutScreenVisible)
            {
                AboutFlipBack.Begin();
                _aboutScreenVisible = false;
                e.Cancel = true;
            }
            if(_musicScreenVisible)
            {
                MusicFlipBack.Begin();
                _musicScreenVisible = false;
                e.Cancel = true;
            }
            foreach (ApplicationBarIconButton button in ApplicationBar.Buttons)
            {
                button.IsEnabled = true;
            }
            base.OnBackKeyPress(e);
        }

 
    }

    public class MyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if((bool)value)
                return Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
 	        throw new NotImplementedException();
        }
    }

            
}