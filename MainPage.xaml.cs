using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Data_Logger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Page data fields
        private Timer timer;                                    // To hold timer object
        MeasureLengthDevice measurementDevice = null;           // Instance of MeasureLengthDevice class
        MainDisplayData displayData = new MainDisplayData();    // Instance of MainDisplayData model
        string unitChoice;                                      // To hold user choice of measurement unit
        bool start = true;                                      // For start/stop collecting button
        
        // Access UI from back end code
        Frame frame = null;
        MainPage page = null;

        public MainPage()
        {
            this.InitializeComponent();
            // timer to collect data and set text box fields every 15 seconds
            timer = new Timer(Timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
            measurementDevice = new MeasureLengthDevice();  // Instantiate MeasureLengthDevice class
            startStopButton.Content = "Start Collecting"; // Set intial text for start button
            startStopButton.Background = new SolidColorBrush(Windows.UI.Colors.Green);  // Set initial background color for start button
            //currentMeasureChoice.Text = "Imperial [in]";   // set initial display
            //altMeasureChoice.Text = "Metric [cm]";         // set initial display

            // Instantiate data display model with current and alternate measures, history and time stamp 
            displayData = new MainDisplayData
            {
                Measurement = measurementDevice.Measurement.ToString(),
                AltMeasurement = measurementDevice.AltMeasurement.ToString(),
                History = measurementDevice.GetRawData().ToString(),
                TimeStamp = measurementDevice.TimeStamp
            };
        }

        // Timer_Tick method to display page data on set timer interval
        private async void Timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (page != null)
                {
                    page.measure.Text = measurementDevice.Measurement.ToString();       // display current measure
                    page.altMeasure.Text = measurementDevice.AltMeasurement.ToString(); // display alt measure
                    timeStamp.Text = measurementDevice.TimeStamp.ToString();            // display time stamp
                }

                displayData.History = measurementDevice.GetRawData().ToString();        // display history
            });
        }

        // startStopButton_Click event handler to start/stop process
        private void startStopButton_Click(object sender, RoutedEventArgs e)
        {
            // if not started, start device and display page info, else stop device
            // toggle button text and background color based on button click
            if (start)
            {
                frame = (Frame)Window.Current.Content;
                page = (MainPage)frame.Content;

                startStopButton.Content = "Stop Collecting";
                startStopButton.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                measurementDevice.StartCollecting();
                start = false;
            } else
            {
                startStopButton.Content = "Start Collecting";
                startStopButton.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                measurementDevice.StopCollecting();
                start = true;
            }

        }

        // displayHistoryButton_Click event handler to display history collection in window
        private void displayHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            measureHistory.Text = measurementDevice.GetRawData().ToString();
        }

        // unitOfMeasure_Checked event handler to set which unit is chosen and change display based on selection
        private void unitOfMeasure_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;             // instantiate Radio button group

            if (rb !=null && currentMeasureChoice != null)
            {
                unitChoice = rb.Tag.ToString();                 // get unit choice as string
                measurementDevice.setUnitsToUse(unitChoice);    // set unit choice in MeasureLengthDevice class

                // decide which unit was selected and how to manipulate display data
                switch (unitChoice)
                {
                    case "Imperial":
                        currentMeasureChoice.Text = unitChoice + " [in]";
                        altMeasureChoice.Text = "Metric [cm]";
                        measure.Text = "0";
                        altMeasure.Text = "0";
                        break;
                    case "Metric":
                        currentMeasureChoice.Text = unitChoice + " [cm]";
                        altMeasureChoice.Text = "Imperial [in]";
                        measure.Text = "0";
                        altMeasure.Text = "0";
                        break;
                }
                
            }            
        }        
    }
}
