using System;
using Foundation;

namespace com.atombooster.roomplus
{
	public class SystemValue
	{
		//Used for the url that the json service seated
		private static string _publicURL = string.Empty;

		public static string PublicURL { 
			get
			{
				if (!_publicURL.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
					_publicURL += "/";

				if (!_publicURL.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
					_publicURL = "http://" + _publicURL;

				return _publicURL;
			}
			set
			{
				_publicURL = value;
			}
		}
		//client id for ceretain device
		public static string ClientId { get; set; }
		//secredcty key for the this client
		public static string SecrectKey { get; set; }


		public static nint AutoScrollInterval { get; set; }

		public static bool AutoScrollMainImage { get; set; }

		static NSUserDefaults plist;

		/// <summary>
		/// Loads the default system value.
		/// </summary>
		public static void LoadDefaultValue()
		{
			if (plist == null)
				plist = new NSUserDefaults("group.com.atombooster.roomplus", NSUserDefaultsType.SuiteName);

			PublicURL = plist.StringForKey("PublicURL");
			SecrectKey = plist.StringForKey("SecrectKey");
			AutoScrollInterval = plist.IntForKey("AutoScrollInterval");
			AutoScrollMainImage = plist.BoolForKey("AutoScrollMainImage");

			ClientId = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.ToString(); //plist.StringForKey("SecrectKey");



			#region Default Value for testing purpose only
			if (EnvironmentDetector.InSimulator())
			{
				PublicURL = "127.0.0.1:8080";
				SecrectKey = "1234567890";
				ClientId = "ABCDEFGHIJKLMN";
				AutoScrollInterval = 3;
				AutoScrollMainImage = true;
			}

			#endregion

		}

		/// <summary>
		/// Saves the default value.
		/// </summary>
		public static void SaveDefaultValue()
		{
			if (plist == null)
				plist = new NSUserDefaults("group.com.atombooster.roomplus", NSUserDefaultsType.SuiteName);

			//Set the value
			plist.SetString(PublicURL, "PublicURL");
			plist.SetString(ClientId, "SecrectKey");
			plist.SetInt(AutoScrollInterval, "AutoScrollInterval");
			plist.SetBool(AutoScrollMainImage, "AutoScrollMainImage");

			//plist.SetString(SecrectKey, "SecrectKey");

			//Save the data
			plist.Synchronize();
		}
	}
}
