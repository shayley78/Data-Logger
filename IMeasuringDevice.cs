
namespace Data_Logger
{
    // Interface for Measuring Device methods
    interface IMeasuringDevice
    {
        // Returns a decimal that represents the metric value
        // of the most recent measurement that was captured
        double MetricValue(int measurement);

        // Returns a decimal that represents the imperial value
        // of the most recent measurement that was captured
        double ImperialValue(int measurement);

        // Starts the device running
        void StartCollecting();

        // Stops the device
        void StopCollecting();

        // Retrieves a copy of all of the recent data that the
        // measuring device has captured.  The data will be
        // returned as an array of integer values.
        string GetRawData();
    }
}
