using System;
namespace Applicable.Location
{
	public class LocationData
	{
		public double Latitude { get; protected set; }
		public double Longtitude { get; protected set; }
		public double Heading { get; protected set; }
		public double Accuracy { get; protected set; }
		public DateTime Timestamp { get; protected set; }
		
		public LocationData (double latitude, double longtitude, double heading, double accuracy, DateTime timestamp)
		{
			Latitude = latitude;
			Longtitude = longtitude;
			Heading = heading;
			Accuracy = accuracy;
			Timestamp = timestamp;
		}
		
		public override string ToString ()
		{
			return string.Format ("[LocationData: Latitude={0}, Longtitude={1}, Heading={2}, Accuracy={3}, Timestamp={4}]", Latitude, Longtitude, Heading, Accuracy, Timestamp);
		}
		
	}
}
