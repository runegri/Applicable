using System;
using Windows.Devices.Geolocation;

namespace Applicable.Location
{
    public class WinRtLocationService : ILocationService
    {
        readonly Geolocator _geolocator;
        bool _isRunning;

        public WinRtLocationService()
        {
            // Report position every 10 sec and with 10 m threshold
            _geolocator = new Geolocator { MovementThreshold = 10, ReportInterval = 10000 };
            _geolocator.PositionChanged += PositionChanged;
        }

        private void PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if(!_isRunning)
            {
                return;
            }
            var locationChanged = LocationChanged;
            if (locationChanged != null)
            {
                locationChanged(ConvertPositionData(args.Position));
            }
        }

        private static LocationData ConvertPositionData(Geoposition position)
        {
            var latitude = position.Coordinate.Latitude;
            var longtitude = position.Coordinate.Longitude;
            var heading = position.Coordinate.Heading;
            var accuracy = position.Coordinate.Accuracy;
            var timestamp = position.Coordinate.Timestamp;

            return new LocationData(latitude, longtitude, heading ?? 0, accuracy, timestamp.LocalDateTime);        

        }

        public Action<LocationData> LocationChanged { get; set; }
        public void Start()
        {
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }
    }
}