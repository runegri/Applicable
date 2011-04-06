using System;
namespace GpsTool
{
	public interface IGpsService
	{
		Action<LocationData> LocationChanged { get; set; }
		void Start();
		void Stop();
	}




	
	
}



