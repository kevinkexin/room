using System;
using Foundation;

namespace RoomPlus
{
	public class SystemValue
	{
		//Used for the url that the json service seated
		public static string PublicURL { get; set; }
		//client id for ceretain device
		public static string ClientId { get; set; }
		//secredcty key for the this client
		public static string SecrectKey { get; set; }

		static NSUserDefaults plist;

		/// <summary>
		/// Loads the default system value.
		/// </summary>
		public static void LoadDefaultValue()
		{
			if (plist == null)
				plist = new NSUserDefaults("group.com.atombooster.roomplus", NSUserDefaultsType.SuiteName);

			PublicURL = plist.StringForKey("PublicURL");
			ClientId = plist.StringForKey("ClientId");
			SecrectKey = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.ToString(); //plist.StringForKey("SecrectKey");


			#region Default Value for testing purpose only
			if (string.IsNullOrWhiteSpace(PublicURL))
				PublicURL = "127.0.01";

			if (string.IsNullOrWhiteSpace(ClientId))
				ClientId = "325684";
			
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
			plist.SetString(ClientId, "ClientId");
			//plist.SetString(SecrectKey, "SecrectKey");

			//Save the data
			plist.Synchronize();
		}
	}
}
