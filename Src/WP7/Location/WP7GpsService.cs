using System;
using System.Device.Location;
using GpsTool;

namespace Applicable
{
    public class Wp7GpsService : IGpsService
    {

        private readonly GeoCoordinateWatcher _watcher;

        public Wp7GpsService()
        {
            _watcher = new GeoCoordinateWatcher();
            _watcher.PositionChanged += PositionChanged;
        }

        void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var locationChanged = LocationChanged;
            if (locationChanged != null)
            {
                locationChanged(ConvertPositionData(e.Position));
            }
        }

        public Action<LocationData> LocationChanged { get; set; }

        public void Start()
        {
            _watcher.Start();
        }

        public void Stop()
        {
            _watcher.Stop();
        }

        public LocationData ConvertPositionData(GeoPosition<GeoCoordinate> position)
        {
            var latitude = position.Location.Latitude;
            var longtitude = position.Location.Longitude;
            var heading = 0d;
            var accuracy = position.Location.HorizontalAccuracy;
            
            return new LocationData(latitude, longtitude, heading, accuracy, position.Timestamp.LocalDateTime);        
        }
    }
}
