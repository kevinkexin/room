using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace com.atombooster.roomplus
{
	[Register("WeatherView"), DesignTimeVisible(true)]
	public class WeatherView : UIView
	{
		public WeatherView(IntPtr handle) : base(handle) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.atombooster.roomplus.WeatherView"/> class.
		/// </summary>
		public WeatherView()
		{
			Initialize();
		}

		/// <summary>
		/// Called when loaded from xib or storyboard.
		/// </summary>
		public override void AwakeFromNib()
		{
			// Initialize the view here.
			Initialize();
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		void Initialize()
		{
			// Common initialization code here.
			WeatherDate = DateTime.Now.Date;
			CurrentTemp = 0;
			MaxTemp = 0;
			MinTemp = 0;
			Condition = string.Empty;

			//1 Add a Image
		}

		#region Properties
		[Export("Date"), Browsable(true)]
		public DateTime WeatherDate { get; set; }

		[Export("CurrentTemp"), Browsable(true)]
		public int CurrentTemp { get; set; }

		[Export("MaxTemp"), Browsable(true)]
		public int MaxTemp { get; set; }

		[Export("MinTemp"), Browsable(true)]
		public int MinTemp { get; set; }

		[Export("Condition"), Browsable(true)]
		public string Condition { get; set; }

		[Export("Language"), Browsable(true)]
		public string Language { get; set; }
		#endregion
	}
}