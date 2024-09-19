using System;
using API.Entities;

namespace API.DTOs
{
	public class AttendanceRecordDto
	{
		public DateTime Date { get; set; }
		public AttendanceStatus Status { get; set; }
		public DateTime? CheckInTime { get; set; }
		public DateTime? CheckOutTime { get; set; }
		public string Location { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
		
	}
}
