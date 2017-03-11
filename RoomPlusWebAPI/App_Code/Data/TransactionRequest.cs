using System;
namespace RoomPlusWebAPI
{
	public class TransactionRequest
	{
		public int ServiceId { get; set; }
		public string ServiceCode { get; set; }
		public string ServiceName { get; set; }
		public string TerminalId { get; set; }
		public string Location { get; set; }
		public string TimeRemak { get; set; }
		public DateTime? StartDatetime { get; set; }
		public DateTime? EndDatetime { get; set; }
		public string Remark { get; set; }
		public RequestStatus Status { get; set; }
		public string AttendedBy { get; set; }
		public DateTime AttendedTime { get; set; }

		public TransactionDetail[] ServiceDetails { get; set; }

		public class TransactionDetail
		{
			public int LineNo { get; set; }
			public int ServiceDetailId { get; set; }
			public string DetailName { get; set; }
			public DateTime? StartDatetime { get; set; }
			public DateTime? EndDatetime { get; set; }
		}
	}
}
