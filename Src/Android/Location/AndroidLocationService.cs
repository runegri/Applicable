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
                Log.Debug(Tag, "Already started, stopping");
                Stop();
            }
            
            _locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 10000, 10, this);
            _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 10000, 10, this);
            
            isStarted = true;
            Log.Debug(Tag, "Started");

            var lastPositionNetwork = _locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);
            var lastPositionGps = _locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
            Android.Locations.Location lastPosition = null;
            lastPosition = LatestPosition(lastPositionNetwork, lastPositionGps);

            if (lastPosition != null)
            {
                Log.Debug(Tag, "Reporting last known position");
                OnLocationChanged(lastPosition);
            }
            else
            {
                Log.Debug(Tag, "No known position");
            }
        }

        private static Android.Locations.Location LatestPosition(Android.Locations.Location lastPositionNetwork, Android.Locations.Location lastPositionGps)
        {
            if (lastPositionNetwork != null && lastPositionGps != null)
            {
                return lastPositionNetwork.Time > lastPositionGps.Time ? lastPositionNetwork : lastPositionGps;
            }
            else if (lastPositionGps != null)
            {
                return lastPositionGps;
            }
            else if (lastPositionNetwork != null)
            {
                return lastPositionNetwork;
            }
            return null;
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
            
            var locationChanged = LocationChanged;
            if (locationChanged != null)
            {
                
                var latitude = location.Latitude;
                var longtitude = location.Longitude;
                var heading = location.Bearing;
                var accuracy = location.Accuracy;
                var startOfEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var timestamp = startOfEpoch.AddMilliseconds(location.Time).ToLocalTime();
                var locationData = new LocationData(latitude, longtitude, heading, accuracy, timestamp);
                Log.Debug(Tag, "Location changed: " + locationData);
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