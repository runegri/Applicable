using System;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
namespace GpsTool
{
	[Preserve(AllMembers=true)]
	public class IOSGpsService : IGpsService
	{

		private CLLocationManager _locationManager;

		public IOSGpsService ()
		{
			_locationManager = new CLLocationManager ();
			_locationManager.Delegate = new GpsListenerDelegate (this);
			_locationManager.DistanceFilter = 20;
			_locationManager.DesiredAccuracy = 10;
		}

		public void Start ()
		{
			_locationManager.StartUpdatingLocation ();
			_locationManager.StartUpdatingHeading ();
		}

		public void Stop ()
		{
			_locationManager.StopUpdatingLocation ();
			_locationManager.StopUpdatingHeading ();
		}

		public Action<LocationData> LocationChanged { get; set; }
		
	}


	internal class GpsListenerDelegate : CLLocationManagerDelegate
	{

		private IOSGpsService _gpsService;
		private CLLocation _lastLocation;
		private CLHeading _lastHeading;

		public GpsListenerDelegate (IOSGpsService gpsService)
		{
			_gpsService = gpsService;
		}

		public override void UpdatedLocation (CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
		{
			_lastLocation = newLocation;
			UpdatedLocationAndHeading ();
		}

		public override void UpdatedHeading (CLLocationManager manager, CLHeading newHeading)
		{
			_lastHeading = newHeading;
			UpdatedLocationAndHeading ();
		}

		public override void Failed (CLLocationManager manager, MonoTouch.Foundation.NSError error)
		{
			// TODO: Implement - see: http://go-mono.com/docs/index.aspx?link=T%3aMonoTouch.Foundation.ModelAttribute
		}

		private void UpdatedLocationAndHeading ()
		{
			var locationChanged = _gpsService.LocationChanged;
			if (locationChanged != null) {
				
				double latitude = 0;
				double longtitude = 0;
				double heading = 0;
				double accuracy = 0;
				
				if (_lastLocation != null && _lastLocation.Coordinate.IsValid()) {
					latitude = _lastLocation.Coordinate.Latitude;
					longtitude = _lastLocation.Coordinate.Longitude;
					accuracy = _lastLocation.HorizontalAccuracy;
				}
				
				if (_lastHeading != null) {
					heading = _lastHeading.TrueHeading;
				}
				
				var locationData = new LocationData (latitude, longtitude, heading, accuracy, DateTime.Now);
				
				locationChanged (locationData);
				
			}
		}
		
	}
	
	
}

