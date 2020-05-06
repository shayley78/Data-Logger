using System;
using System.Text;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Data_Logger
{
    // Implements the IMeasuringDevice interface and uses Device object to start collecting data
    class MeasureLengthDevice: IMeasuringDevice
    {
        // Private class fields
        private Units unitsToUse;                       // To hold user defined measurement interpretation (Imperial/Metric)
        private FixedQueue<int> dataCaptured = null;    // To store a history of a limited set of recently captured measurements
        private int mostRecentMeasure = 0;              // To store the most recent measurement captured
        private double convertedMeasure = 0.0;          // To store the converted/alternate measurement
        private Device myDevice = new Device();         // To give access to Device class public methods
        private Timer timer;                            // Timer object to call method in chosen increment
        private const double conversionRate = 2.54;     // Impertial to metric conversion rate
        private DateTime timeStamp;                     // DateTime object for timestamp

        // Constructor to initialize fields
        public MeasureLengthDevice()
        {
            dataCaptured = new FixedQueue<int>();
            dataCaptured.Limit = 10;
            this.unitsToUse = Units.Imperial;
        }

        // Set units to use according to user input
        public void setUnitsToUse(String u)
        {
            this.unitsToUse = (Units)Enum.Parse(typeof(Units), u);
        }

        #region // Give public access to most recent measurement, alt measurement and time stamp
        public int Measurement
        {
            get { return this.mostRecentMeasure; }
        }

        public double AltMeasurement
        {
            get { return this.convertedMeasure; }
        }

        public DateTime TimeStamp
        {
            get { return this.timeStamp; }
        }
        #endregion

        #region// Interface implementation methods

        // GetRawData method to collect and return the contents of measurement collection
        public string GetRawData()
        {
            StringBuilder myString = new StringBuilder();
            foreach (var i in dataCaptured.q)
                myString.AppendLine(i.ToString());
            return myString.ToString();
        }

        // ImperialValue method to return the most recent measurement, and convert it if unitsToUse is not Imperial
        public double ImperialValue(int measurement)
        {
            double impValue = measurement;
            if (this.unitsToUse != Units.Imperial)
            {
                impValue = measurement / conversionRate;
            }

            return impValue;
        }

        // MetricValue method to return the most recent measurement, and convert it if unitsToUse is not Metric
        public double MetricValue(int measurement)
        {
            double metValue = measurement;
            if (this.unitsToUse != Units.Metric)
            {
                metValue = measurement * conversionRate;
            }

            return metValue;
        }

        // StartCollecting method starts a timer and calls Timer_Tick method to start capturing data every 15 seconds
        public void StartCollecting()
        {
            timer = new Timer(Timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
        }

        // Stop collecting data by disposing of the timer
        public void StopCollecting()
        {
            timer.Dispose();
        }
        #endregion

        // Timer_Tick method sets most recent measure with a new value and add it to the measurement collection
        // Set converted/alternate measure based on user input of measurement unit, get time stamp for data
        private async void Timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Units unitChoice = unitsToUse;                  // get measurement unit
                mostRecentMeasure = myDevice.GetMeasurement;    // set most recent measure
                // determine which alternate measurement is calculated based on unti choice
                if (unitChoice == Units.Imperial)
                {
                    convertedMeasure = MetricValue(mostRecentMeasure);  // if Imperial set alt measure to metric
                }
                else
                {
                    convertedMeasure = ImperialValue(mostRecentMeasure); // else set alt measure to imperial
                }
                dataCaptured.Enqueue(mostRecentMeasure);        // add standard measure to collection
                timeStamp = DateTime.Now;                       // set time stamp
            });
        }
    }
}
