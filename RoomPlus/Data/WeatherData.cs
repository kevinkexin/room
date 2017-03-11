using System;
namespace com.atombooster.roomplus
{
	/// <summary>
	/// Weather data contains all the weather data information to display on the main page
	/// </summary>
	public class WeatherData
	{
		public WeatherData()
		{
		}

		public WeatherData(int SID, int DayIndex, int CityId, string CityName, string Tag, DateTime WeatherDay, string Condition, int MaxTemp, int MinTemp, string Code, string Icon, string Language)
		{
			this.SID = SID;
			this.DayIndex = DayIndex;
			this.CityId = CityId;
			this.CityName = CityName;
			this.Tag = Tag;
			this.WeatherDay = WeatherDay;
			this.Condition = Condition;
			this.MaxTemp = MaxTemp;
			this.MinTemp = MinTemp;
			this.Code = Code;
			this.Icon = Icon;
			this.Language = Language;
		}

		public int DayIndex { get; set; }
		public int SID { get; set; }
		public int CityId { get; set; }
		public string CityName { get; set; }
		public string Tag { get; set; }
		public DateTime WeatherDay { get; set; }
		public string Condition { get; set; }
		public int MaxTemp { get; set; }
		public int MinTemp { get; set; }
		public string Code { get; set; }
		public string Language { get; set; }
		private string _Icon = string.Empty;
		public string Icon
		{
			set
			{
				_Icon = value;
			}
			get
			{
				if (_Icon.Length > 2)
					return _Icon.Substring(0, 2);
				else
					return _Icon;
			}
		}
	}
}
