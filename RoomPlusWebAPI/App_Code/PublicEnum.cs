using System;
namespace RoomPlusWebAPI
{
	public enum RequestStatus
	{
		Sent = 0, //Send to the Server
		Printed = 1, //printed at the server
		Attended = 2, //Responsed by the server
		Ack = 3 //Acked by the client PC
	}
}
