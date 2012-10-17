using System;
using Android.Content;
using Android.Locations;
using Android.OS;

namespace Applicable.Location
{
    public class AndroidLocationService : Java.Lang.Object, ILocationService, ILocationListener
    {
        private readonly LocationManager _locationManager;
        private readonly Context _activity;
        private bool _isStarted;
        private bool _isPaused;
       
        public AndroidLocationService(Context activity)
        {
            _activity = activity;
            _locationManager = (LocationManager)_activity.GetSystemService(Context.LocationService);
        }

        public Action<LocationData> LocationChanged { get; set; }

        public void Start()
        {
            _isPaused = false;
            if (_isStarted)
            {
                return;
            }

            if (_locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
                _locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 1000, 0, this);
            if (_locationManager.IsProviderEnabled(LocationManager.GpsProvider))
            _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 1000, 0, this);
            _isStarted = true;

            var lastPositionNetwork = _locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);
            var lastPositionGps = _locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
            Android.Locations.Location lastPosition;
            lastPosition = LatestPosition(lastPositionNetwork, lastPositionGps);

            if (lastPosition != null)
            {
                OnLocationChanged(lastPosition);
            }
        }

        public bool IsLocationServiceEnabled()
        {
            return _locationManager.IsProviderEnabled(LocationManager.GpsProvider) 
                || _locationManager.IsProviderEnabled(LocationManager.NetworkProvider);
        }

        
        private static Android.Locations.Location LatestPosition(Android.Locations.Location lastPositionNetwork, Android.Locations.Location lastPositionGps)
        {
            if (lastPositionNetwork != null && lastPositionGps != null)
            {
                return lastPositionNetwork.Time > lastPositionGps.Time ? lastPositionNetwork : lastPositionGps;
            }
            if (lastPositionGps != null)
            {
                return lastPositionGps;
            }
            if (lastPositionNetwork != null)
            {
                return lastPositionNetwork;
            }
            return null;
        }

        public void Stop()
        {
            InternalStop();
            _isStarted = false;
            _isPaused = false;
        }

        private void InternalStop()
        {
            if (!_isStarted)
            {
                //Log.Debug(Tag, "Already stopped");
                return;
            }
            _locationManager.RemoveUpdates(this);
        }

        public void Pause()
        {
            if (_isPaused)
            {
                System.Diagnostics.Debug.Assert(!_isStarted);
                return;
            }
            if (_isStarted)
            {
                InternalStop();
                _isStarted = false;
                _isPaused = true;
            }
            else
            {
                _isPaused = false;
            }
        }

        public void Resume()
        {
            if (_isPaused)
            {
                _isPaused = false;
                if (_isStarted)
                {
                    return;
                }
                Start();
            }
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
                locationChanged(locationData);
            }

        }

        public void OnProviderDisabled(string provider)
        { }

        public void OnProviderEnabled(string provider)
        { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        { }

        #endregion

    }

}