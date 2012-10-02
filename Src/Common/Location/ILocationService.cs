using System;

namespace Applicable.Location
{
    public interface ILocationService
    {
        Action<LocationData> LocationChanged { get; set; }
        void Start();
        void Stop();
    }
}



