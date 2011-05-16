using System;
using MonoTouch.Foundation;
namespace Applicable.Location
{
	[Preserve(AllMembers=true)]
	public class DebugLocationService : ILocationService
	{
		
		NSTimer _timer;
		bool _running;
		
		double _latitude, _longtitude;
		
		public DebugLocationService ()
		{
			_latitude = 63.425630295;
			_longtitude = 10.4458852325;
			_timer = NSTimer.CreateRepeatingScheduledTimer(1, TimerEvent);
		}
		
		private void TimerEvent()
		{
			if(_running)
			{
				var loc = new LocationData(_latitude, _longtitude, 0, 1, DateTime.Now);
				if(LocationChanged != null)
				{
					LocationChanged(loc);
				}
				_latitude += 0.000001;
				_longtitude += 0.000001;
			}
		}
		
		#region IGpsService implementation
		public void Start ()
		{
			_running = true;
		}

		public void Stop ()
		{
			_running = false;
		}

		public Action<LocationData> LocationChanged {
			get; set;
		}
		#endregion
}
}

