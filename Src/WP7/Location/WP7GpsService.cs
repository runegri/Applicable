//#define GPS_EMULATOR
using System;
using System.Device.Location;
using GpsEmulatorClient.ServiceReference1;

namespace Applicable.Location
{
    public class Wp7LocationService : ILocationService
    {

        //private readonly GeoCoordinateWatcher _watcher;
        private readonly IGeoPositionWatcher<GeoCoordinate> _watcher;
        public Wp7LocationService()
        {
            //_watcher = new GeoCoordinateWatcher();
           // #if GPS_EMULATOR
               // _watcher = new GpsEmulatorClient.GeoCoordinateWatcher();
           // #else
                _watcher = new System.Device.Location.GeoCoordinateWatcher();
           // #endif

            //_watcher.PositionChanged += PositionChanged;
                _watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(PositionChanged);
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
            //double latitude = 63.425630295;
            //double longitude = 10.4458852325;
            //double heading = 0d;
            //double accuracy = 0.0;

            //if (!position.Location.IsUnknown)
            //{ 
            //    latitude = position.Location.Latitude;
            //    longitude = position.Location.Longitude;
            //    accuracy = position.Location.HorizontalAccuracy;
            //}
            
            return new LocationData(latitude, longtitude, heading, accuracy, position.Timestamp.LocalDateTime);        
        }
    }
}
