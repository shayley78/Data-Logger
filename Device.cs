using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Data_Logger
{
    // Device class to generate data in one second increments
    class Device
    {
        // Private class fields
        private Timer timer;                // Timer object to call method in chosen increment
        private int data = 0;               // To hold random integer
        private Random rnd = new Random();  // To generate random data
        public Device()
        {
            // Instantiate timer, call Timer_Tick method, wait 1 second to start, tick every one second afterward
            timer = new Timer(Timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(1).TotalMilliseconds);
        }

        // Timer_Tick method to generate random integer between one and ten.
        private async void Timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                data = rnd.Next(1, 11);
            });
        }

        // Give public access to random integer
        public int GetMeasurement => data;

    }
}
