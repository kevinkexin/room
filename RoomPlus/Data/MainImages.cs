using System;
namespace com.atombooster.roomplus
{
	/// <summary>
	/// Main images used to display on the main screen.
	/// </summary>
	public class MainImages
	{
		public MainImages(int SID, string PicName, string PicURL, int ServiecId, int DetailId, string Remark, DateTime LastModifyDate)
		{
			this.SID = SID;
			this.PicName = PicName;
			this.PicURL = PicURL;
			this.ServiceId = ServiceId;
			this.DetailId = DetailId;
			this.Remark = Remark;
			this.LastModifyDate = LastModifyDate;
		}

		public int SID { get; set; }
		public DateTime LastModifyDate { get; set; }
		public string PicName { get; set; }
		public string PicURL { get; set; }
		public int ServiceId { get; set; }
		public int DetailId { get; set; }
		public string Remark { get; set; }
	}
}
