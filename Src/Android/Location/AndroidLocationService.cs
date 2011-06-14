using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;

namespace Applicable.Location
{
    public class AndroidLocationService : Java.Lang.Object, ILocationService, ILocationListener
    {

        private const string Tag = "AndroidLocationService";

        private readonly LocationManager _locationManager;
        private readonly Activity _activity;
        private bool isStarted = false;
       
        public AndroidLocationService(Activity activity)
        {
            _activity = activity;
            _locationManager = (LocationManager)_activity.GetSystemService(Context.LocationService);
            
            Log.Debug(Tag, "Created");
        }

        public Action<LocationData> LocationChanged { get; set; }

        public void Start()
        {
            if (isStarted)
            {
                Log.Debug(Tag, "Already started");
                return;
            }
            var locationProvider = _locationManager.GetBestProvider(new Criteria {Accuracy = Accuracy.Coarse}, true);
            _locationManager.RequestLocationUpdates(locationProvider, 10000, 10, this);
            var lastPosition = _locationManager.GetLastKnownLocation(locationProvider);
            if(lastPosition != null)
            {
                OnLocationChanged(lastPosition);
            }
            isStarted = true;
            Log.Debug(Tag, "Started");
        }

        public void Stop()
        {
            if (!isStarted)
            {
                Log.Debug(Tag, "Already stopped");
                return;
            }
            _locationManager.RemoveUpdates(this);
            isStarted = false;
            Log.Debug(Tag, "Stopped");
        }

        #region Implementation of ILocationListener

        public void OnLocationChanged(Android.Locations.Location location)
        {
            Log.Debug(Tag, "Location changed");
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

    }

}