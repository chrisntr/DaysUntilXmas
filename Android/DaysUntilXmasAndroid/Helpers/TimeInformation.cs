using System;
using System.ComponentModel;
using System.Text;

namespace DaysUntilXmasAndroid
{
	public class TimeInformation : INotifyPropertyChanged
	{
		private string _daysUntil;
		public string DaysUntil
		{
			get { return _daysUntil; }
			set
			{
				_daysUntil = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("DaysUntil"));
			}
		}

		private string _hoursUntil;
		public string HoursUntil
		{
			get { return _hoursUntil; }
			set
			{
				_hoursUntil = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("HoursUntil"));
			}
		}

		private string _minutesUntil;
		public string MinutesUntil
		{
			get { return _minutesUntil; }
			set
			{
				_minutesUntil = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("MinutesUntil"));
			}
		}

		private string _secondsUntil;
		public string SecondsUntil
		{
			get { return _secondsUntil; }
			set
			{
				_secondsUntil = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("SecondsUntil"));
			}
		}

		public override string ToString()
		{
			return String.Format("Seconds = {0}, Minutes = {1}, Hours = {2}, Days = {3}", SecondsUntil, MinutesUntil,
			                     HoursUntil, DaysUntil);
		}


		#region INotifyPropertyChanged Members


		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));



		}
		#endregion


		public event PropertyChangedEventHandler PropertyChanged;
	}
}