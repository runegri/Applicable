using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Applicable.Location
{
    
    public class WP7SimulatorLocation : ILocationService
    {
        DispatcherTimer _timer;
        double _longtitude;
        double _latitude;
        bool _running;

        public WP7SimulatorLocation()
        {
            _latitude = 63.425630295;
            _longtitude = 10.4458852325;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {            
            if (_running)
            {
                var loc = new LocationData(_latitude, _longtitude, 0, 1, DateTime.Now);
                if (LocationChanged != null)
                {
                    LocationChanged(loc);
                }
                _latitude += 0.000001;
                _longtitude += 0.000001;
            }
        }

        public void Start()
        {
            _running = true;            
        }

        public void Stop()
        {
            _running = false;
        }

        public Action<LocationData> LocationChanged
        {
            get;
            set;
        }
    }
}
