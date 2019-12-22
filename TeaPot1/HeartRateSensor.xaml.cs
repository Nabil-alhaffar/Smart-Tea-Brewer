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
    public partial class HeartRateSensor : CirclePage
    {
        private HeartRateMonitor _monitor;
        private bool _measuring = false;

        public HeartRateSensor()
        {
            InitializeComponent();
            CheckPrivileges();
        }
        private void OnActionButtonClicked(object sender, EventArgs e)
        {
            if (_measuring)
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
            _monitor = new HeartRateMonitor();
            // specify frequency of the sensor data event by setting the interval value (in milliseconds)
            _monitor.Interval = 1000;
            MessagingCenter.Subscribe<Application>(this, "sleep", (sender) => { if (_measuring) { StopMeasurement(); } });
            MessagingCenter.Subscribe<Application>(this, "resume", (sender) => { if (!_measuring) { StartMeasurement(); } });
           
        }
        private void OnPrivilegesDenied()
        {
            // close the application
            Tizen.Applications.Application.Current.Exit();
        }

        private void OnMonitorDataUpdated(object sender, HeartRateMonitorDataUpdatedEventArgs e)
        {
            // update displayed value
            hrValue.Text = e.HeartRate > 0 ? e.HeartRate.ToString() : "0";
        }

        private void StartMeasurement()
        {
            _monitor.DataUpdated += OnMonitorDataUpdated;
            _monitor.Start();
            _measuring = true;

            // update the view
            actionButton.Text = "STOP";
            measuringIndicator.IsVisible = true;
        }

        private void StopMeasurement()
        {
            _monitor.DataUpdated -= OnMonitorDataUpdated;
            _monitor.Stop();
            _measuring = false;

            // update the view
            actionButton.Text = "MEASURE";
            measuringIndicator.IsVisible = false;
        }
    }
}