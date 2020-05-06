using System;
using System.ComponentModel;

namespace Data_Logger
{
    // View model for main display
    class MainDisplayData
    {
        // Private class fields
        private string measurement;     // to hold current measurement
        private string altMeasurement;  // to hold alternate measurement
        private string history;         // to hold history
        private DateTime timeStamp;     // to hold time stamp

        #region // to give public access to members
        public string History
        {
            get { return this.history; }
            set { this.history = value; }
        }

        public string Measurement
        {
            set
            {
                if (measurement != value)
                {
                    OnPropertyChanging("Measurement");
                    measurement = value;
                    OnPropertyChanged("Measurement");
                }
            }
            get
            {
                return measurement;
            }
        }

        public string AltMeasurement
        {
            set
            {
                if (altMeasurement != value)
                {
                    OnPropertyChanging("AltMeasurement");
                    altMeasurement = value;
                    OnPropertyChanged("AltMeasurement");
                }
            }
            get
            {
                return altMeasurement;
            }
        }

        public DateTime TimeStamp
        {
            set
            {
                if (timeStamp != value)
                {
                    OnPropertyChanging("TimeStamp");
                    timeStamp = value;
                    OnPropertyChanged("TimeStamp");
                }
            }
            get
            {
                return timeStamp;
            }
        }
        #endregion

        // Events for data binding
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        #region // Methods to fire when events occur in view model
        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging.Invoke(this, new PropertyChangingEventArgs(propertyName));
            } 
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
