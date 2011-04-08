using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;

namespace Applicable.Location
{
    public class AndroidLocationService : ILocationService, ILocationListener
    {

        private readonly LocationManager _locationManager;
        private readonly Activity _activity;

        public AndroidLocationService(Activity activity)
        {
            _activity = activity;
            _locationManager = (LocationManager)_activity.GetSystemService(Context.LocationService);
        }

        public Action<LocationData> LocationChanged { get; set; }

        public void Start()
        {
            var locationProvider = _locationManager.GetBestProvider(new Criteria {Accuracy = Accuracy.Coarse}, true);
            _locationManager.RequestLocationUpdates(locationProvider, 1000, 10, this);
            var lastPosition = _locationManager.GetLastKnownLocation(locationProvider);
            if(lastPosition != null)
            {
                OnLocationChanged(lastPosition);
            }
        }

        public void Stop()
        {
            _locationManager.RemoveUpdates(this);
        }

        #region Implementation of ILocationListener

        public void OnLocationChanged(Android.Locations.Location location)
        {
            var locationChanged = LocationChanged;
            if (locationChanged != null)
            {
                var latitude = location.Latitude;
                var longtitude = location.Longitude;
                var heading = location.Bearing;
                var accuracy = location.Accuracy;
                var timestamp = DateTime.FromFileTimeUtc(location.Time);

                var locationData = new LocationData(latitude, longtitude, heading, accuracy, timestamp);

                locationChanged(locationData);
            }

        }

        public void OnProviderDisabled(string provider)
        { }

        public void OnProviderEnabled(string provider)
        { }

        public void OnStatusChanged(string provider, int status, Bundle extras)
        { }

        #endregion

        #region Implementation of IJavaObject

        public IntPtr Handle
        {
            get { return _activity.Handle; }
            set { }
        }
        #endregion
    }
}