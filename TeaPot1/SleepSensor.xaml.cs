using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tizen.Security;
using Tizen.Sensor;
using Tizen.Wearable.CircularUI.Forms;


namespace TeaPot1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SleepSensor : CirclePage
    {

        private SleepMonitor _sleepMonitor;
        private bool _sleepMeasuring = false;

        public SleepSensor()
        {

            InitializeComponent();
            CheckPrivileges();

        }
        private void OnActionButtonClicked(object sender, EventArgs e)
        {
            if (_sleepMeasuring)
            {
                StopMeasurement();
            }
            else
            {
                StartMeasurement();
            }
        }
        private void CheckPrivileges()
        {
            // check permission status (allow, deny, ask) to determine action which has to be taken
            string privilege = "http://tizen.org/privilege/healthinfo";
            CheckResult result = PrivacyPrivilegeManager.CheckPermission(privilege);

            if (result == CheckResult.Allow)
            {
                OnPrivilegesGranted();
            }
            else if (result == CheckResult.Deny)
            {
                OnPrivilegesDenied();
            }
            else // the user must be asked about granting the privilege
            {
                PrivacyPrivilegeManager.GetResponseContext(privilege).TryGetTarget(out var context);

                if (context != null)
                {
                    context.ResponseFetched += (sender, e) =>
                    {
                        if (e.cause == CallCause.Answer && e.result == RequestResult.AllowForever)
                        {
                            OnPrivilegesGranted();
                        }
                        else
                        {
                            OnPrivilegesDenied();
                        }
                    };
                }

                PrivacyPrivilegeManager.RequestPermission(privilege);
            }
        }
        private void OnPrivilegesGranted()
        {
            // create an instance of the monitor
            _sleepMonitor = new SleepMonitor();
            // specify frequency of the sensor data event by setting the interval value (in milliseconds)
            _sleepMonitor.Interval = 1000;
            MessagingCenter.Subscribe<Application>(this, "sleep", (sender) => { if (_sleepMeasuring) { StopMeasurement(); } });
        }
        private void OnPrivilegesDenied()
        {
            // close the application
            Tizen.Applications.Application.Current.Exit();
        }

        private void OnMonitorDataUpdated(object sender, SleepMonitorDataUpdatedEventArgs e)
        {

            // update displayed value
            sleepStateTb.Text = e.SleepState >SleepMonitorState.Unknown ? e.SleepState.ToString() : "Unknown";
        }
        private void StartMeasurement()
        {
            _sleepMonitor.DataUpdated += OnMonitorDataUpdated;
            _sleepMonitor.Start();
            _sleepMeasuring = true;

            // update the view
            actionButton.Text = "STOP";
            measuringIndicator.IsVisible = true;
        }

        private void StopMeasurement()
        {
            _sleepMonitor.DataUpdated -= OnMonitorDataUpdated;
            _sleepMonitor.Stop();
            _sleepMeasuring = false;

            // update the view
            actionButton.Text = "MEASURE";
            measuringIndicator.IsVisible = false;
        }

    }
}