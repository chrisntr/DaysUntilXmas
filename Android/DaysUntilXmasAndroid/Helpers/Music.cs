using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DaysUntilXmasAndroid.Helpers
{
	public class MusicItem 
	{

		public string Name { get; set; }
		public string Path { get; set; }
		public int ResId { get; set; }
	}

	public class MusicOptions
	{
		public List<MusicItem> MusicItems { get; set; }

		public MusicOptions()
		{
			MusicItems = new List<MusicItem>
			{
				new MusicItem(){ Name = "We Wish You A Merry Christmas", Path = "We_Wish_You.mp3", ResId = Resource.Raw.We_Wish_You},
				new MusicItem(){ Name = "Jingle Bells", Path = "Jingle_Bells.mp3", ResId = Resource.Raw.Jingle_Bells},
				new MusicItem(){ Name = "Oh, Christmas Tree", Path = "Oh_Xmas.mp3", ResId = Resource.Raw.Oh_Xmas},
				new MusicItem(){ Name = "Deck the Halls", Path="Deck_The_Halls.mp3", ResId = Resource.Raw.Deck_The_Halls},
				new MusicItem(){ Name = "Oh, Little Town of Bethlehem", Path="Bethlehem.mp3", ResId = Resource.Raw.Bethlehem},
				new MusicItem(){ Name = "No Music"},
			};
				
		}
			
	}
}

