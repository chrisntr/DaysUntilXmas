using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;

namespace DaysUntilChristmas
{
    public class MusicItem : INotifyPropertyChanged
    {
        private bool _defaultOption;
        public bool DefaultOption
        {
            get { return _defaultOption; }
            set
            {
                _defaultOption = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DefaultOption"));
            }
        }

        public string Name { get; set; }
        public string Path { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class MusicOptions
    {
        public List<MusicItem> MusicItems { get; set; }

        public MusicOptions()
        {
            MusicItems = new List<MusicItem>
            {
                new MusicItem(){ Name = "We Wish You A Merry Christmas", Path = "/Music/We_Wish_You.mp3"},
                new MusicItem(){ Name = "Jingle Bells", Path = "/Music/Jingle_Bells.mp3"},
                new MusicItem(){ Name = "Oh, Christmas Tree", Path = "/Music/Oh_Xmas.mp3"},
                new MusicItem(){ Name = "Deck the Halls", Path="/Music/Deck_The_Halls.mp3"},
                new MusicItem(){ Name = "Oh, Little Town of Bethlehem", Path="/Music/Bethlehem.mp3"}
            };

            if (IsolatedStorageSettings.ApplicationSettings.Contains("musicSelection"))
            {
                var selected = (int)IsolatedStorageSettings.ApplicationSettings["musicSelection"];
                MusicItems[selected].DefaultOption = true;
            }
            else
                MusicItems[0].DefaultOption = true;
        }


        public void SetDefaultMusic(int option)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("musicSelection"))
                IsolatedStorageSettings.ApplicationSettings["musicSelection"] = option;
            else
                IsolatedStorageSettings.ApplicationSettings.Add("musicSelection", option);
            IsolatedStorageSettings.ApplicationSettings.Save();

            foreach(var musicItem in MusicItems)
            {
                musicItem.DefaultOption = false;
            }
            Debug.WriteLine("Option = " + option);
            MusicItems[option].DefaultOption = true;
        }
    }
}
