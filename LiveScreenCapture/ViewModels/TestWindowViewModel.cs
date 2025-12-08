using CatenaLogic.Windows.Presentation.WebcamPlayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveScreenCapture.ViewModels
{
    public sealed class TestWindowViewModel
    {
        static TestWindowViewModel()
        {
            s_capPlayer = new CapPlayer();
        }

        public CapPlayer CapPlayer => s_capPlayer;

        public void Start(FilterInfo info)
        {
            if (s_capPlayer.Device != null)
                return;

            s_capPlayer.Device = new CapDevice(info.MonikerString) { MaxHeightInPixels = 720, };
        }

        public void Stop()
        {
            var oldDevice = s_capPlayer.Device;
            s_capPlayer.Device = null;
            oldDevice?.Dispose();
        }



        static readonly CapPlayer s_capPlayer;
    }
}
