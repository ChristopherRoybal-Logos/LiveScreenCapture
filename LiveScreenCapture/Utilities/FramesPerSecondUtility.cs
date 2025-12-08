using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveScreenCapture.Utilities
{
    public static class FramesPerSecondUtility
    {
		// 334ms ~ 3 fps
		// 200ms ~ 5 fps
		// 67ms ~ 15 fps
		// 34ms ~ 30 fps
		// 17ms ~ 60 fps
		
		public static double MillisecondsFromFPS(double fps)
		{
			double msPerSecond = 1000;
			return msPerSecond / fps;
		}
	}
}
