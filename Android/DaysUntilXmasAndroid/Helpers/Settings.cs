/*
 * MvxSettings:
 * Copyright (C) 2013 Refractored: 
 * 
 * Contributors:
 * http://github.com/JamesMontemagno
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */

using System;
using Android.App;
using Android.Content;
using Android.Preferences;

namespace DaysUntilXmasAndroid.Helpers
{


	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		/// <summary>
		/// Simply setup your settings once when it is initialized.
		/// </summary>
		private static AndroidSettings m_Settings;
		private static AndroidSettings AppSettings
		{
			get
			{
				return m_Settings ?? (m_Settings = new AndroidSettings());
			}
		}

		#region Setting Constants

		private const string MusicChoiceKey = "music_choice";
		private const int MusicChoiceDefault = 0;
		private const string MusicTimeStampKey = "music_timestampe";
		private const int MusicTimeStampDefault = 0;
		#endregion

		public static int MusicChoice
		{
			get
			{
				return AppSettings.GetValueOrDefault(MusicChoiceKey, MusicChoiceDefault);
			}
			set
			{
				if (AppSettings.AddOrUpdateValue(MusicChoiceKey, value))
					AppSettings.Save();
			}
		}

		public static int MusicTimeStamp
		{
			get
			{
				return AppSettings.GetValueOrDefault(MusicTimeStampKey, MusicTimeStampDefault);
			}
			set
			{
				if (AppSettings.AddOrUpdateValue(MusicTimeStampKey, value))
					AppSettings.Save();
			}
		}


	}

	public class AndroidSettings
	{
		private static ISharedPreferences SharedPreferences { get; set; }
		private static ISharedPreferencesEditor SharedPreferencesEditor { get; set; }
		private readonly object m_Locker = new object();

		public AndroidSettings()
		{
			SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			SharedPreferencesEditor = SharedPreferences.Edit();

		}

		/// <summary>
		/// Gets the current value or the default that you specify.
		/// </summary>
		/// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
		/// <param name="key">Key for settings</param>
		/// <param name="defaultValue">default value if not set</param>
		/// <returns>Value or default</returns>
		public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
		{
			lock (m_Locker)
			{
				Type typeOf = typeof (T);
				if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof (Nullable<>))
				{
					typeOf = Nullable.GetUnderlyingType(typeOf);
				}
				object value = null;
				var typeCode = Type.GetTypeCode(typeOf);
				switch (typeCode)
				{
				case TypeCode.Boolean:
					value = SharedPreferences.GetBoolean(key, Convert.ToBoolean(defaultValue));
					break;
				case TypeCode.Int64:
					value = SharedPreferences.GetLong(key, Convert.ToInt64(defaultValue));
					break;
				case TypeCode.String:
					value = SharedPreferences.GetString(key, Convert.ToString(defaultValue));
					break;
				case TypeCode.Int32:
					value = SharedPreferences.GetInt(key, Convert.ToInt32(defaultValue));
					break;
				case TypeCode.Single:
					value = SharedPreferences.GetFloat(key, Convert.ToSingle(defaultValue));
					break;
				case TypeCode.DateTime:
					var ticks = SharedPreferences.GetLong(key, -1);
					if (ticks == -1)
						value = defaultValue;
					else
						value = new DateTime(ticks);
					break;
				}

				return null != value ? (T) value : defaultValue;
			}
		}

		/// <summary>
		/// Adds or updates a value
		/// </summary>
		/// <param name="key">key to update</param>
		/// <param name="value">value to set</param>
		/// <returns>True if added or update and you need to save</returns>
		public bool AddOrUpdateValue(string key, object value)
		{
			lock (m_Locker)
			{
				Type typeOf = value.GetType();
				if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof (Nullable<>))
				{
					typeOf = Nullable.GetUnderlyingType(typeOf);
				}
				var typeCode = Type.GetTypeCode(typeOf);
				switch (typeCode)
				{
				case TypeCode.Boolean:
					SharedPreferencesEditor.PutBoolean(key, Convert.ToBoolean(value));
					break;
				case TypeCode.Int64:
					SharedPreferencesEditor.PutLong(key, Convert.ToInt64(value));
					break;
				case TypeCode.String:
					SharedPreferencesEditor.PutString(key, Convert.ToString(value));
					break;
				case TypeCode.Int32:
					SharedPreferencesEditor.PutInt(key, Convert.ToInt32(value));
					break;
				case TypeCode.Single:
					SharedPreferencesEditor.PutFloat(key, Convert.ToSingle(value));
					break;
				case TypeCode.DateTime:
					SharedPreferencesEditor.PutLong(key, ((DateTime)(object)value).Ticks);
					break;
				}
			}

			return true;
		}

		/// <summary>
		/// Saves out all current settings
		/// </summary>
		public void Save()
		{
			lock (m_Locker)
			{
				SharedPreferencesEditor.Commit();
			}
		}

	}
}
