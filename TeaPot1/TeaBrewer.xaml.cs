using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tizen.Security;
using Tizen.Sensor;
using Tizen.Network.WiFi;
using System.Net.Sockets;
using System.Net;
using Tizen.Network.Bluetooth;
using Tizen.Applications;

namespace TeaPot1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeaBrewer : IndexPage
    {
        IEnumerable<BluetoothGattCharacteristic> charc_list= null;
        BluetoothGattCharacteristic selectedcharacteristic = null;
        BluetoothGattService selectedService = null;
        public static string ServiceUUID = "12345678-1234-5678-1234-56789abcdef0";
        public static string CharacteristicUUID = "12345678-1234-5678-1234-56789abcdef1";
        IEnumerable<BluetoothGattService> srv_list;
        static bool StateChanged_flag = false;
        static int controlButton=0;
        static BluetoothLeAdvertiser advertiser= BluetoothAdapter.GetBluetoothLeAdvertiser();
        static BluetoothLeAdvertiseData advertiseData = null;
        static bool advertisingFlag = false;
        public static string remote_addr = "B8:27:EB:DC:D9:EC";
        public  static List<Device> DeviceList = new List<Device>();
        public BluetoothGattClient GattClient = null;
        private HeartRateMonitor _monitor;
        private bool _measuring = false;
        private TemperatureSensor _tSensor;
        private bool _measuringTemperature = false;
        public static Device device = null;
        private static bool scanFlag = false;
        public TeaBrewer()
        {
            InitializeComponent();
            CheckPrivileges();
            if (_measuring)
            {
                StopMeasurement();
            }
            else
            {
                StartMeasurement();
            }
        }

        private void RecommendTea_Clicked(object sender, EventArgs e)
        {
            index_Page.Children[1].IsVisible = true;
            index_Page.CurrentPage = index_Page.Children[1];
            Tea suggestedTea;
            int heartrateFactor =  Convert.ToInt32(heartRateTb.Text);
            int temperatureFactor = Convert.ToInt32(TemperatureSensorTb.Text);
            if (heartrateFactor>=70 && heartrateFactor<90 && temperatureFactor>=70)
                 suggestedTea = new Tea("Green Tea", 4, 4, 70,13);
            else if (heartrateFactor>90 && heartrateFactor<120 || temperatureFactor<70)
                suggestedTea= new Tea("Rooibos Tea", 3, 7, 100,11);
            else if (heartrateFactor>=120 && temperatureFactor<70)
                  suggestedTea = new Tea("Hibiscus Tea", 2, 3, 100,11);
            else 
                suggestedTea= new Tea("Lemon Verbena Tea", 1, 7, 100,11);
            TeaSuggestionTb.Text = suggestedTea.teaName;
            controlButton = suggestedTea.controlButton;

        }

        private void BrewButton_Clicked(object sender, EventArgs e)
        {
            
            Toast.DisplayText("Your tea is brewing!");
            
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
            if (TemperatureSensor.IsSupported)
            {
                _tSensor = new TemperatureSensor();
                _tSensor.Interval = 4000;
            }
            // create an instance of the monitor
            _monitor = new HeartRateMonitor();
            // specify frequency of the sensor data event by setting the interval value (in milliseconds)
            _monitor.Interval = 1000;
            MessagingCenter.Subscribe<Xamarin.Forms.Application>(this, "sleep", (sender) => { if (_measuring) { StopMeasurement(); } });
            MessagingCenter.Subscribe<Xamarin.Forms.Application>(this, "resume", (sender) => { if (!_measuring) { StartMeasurement(); } });

        }
        private void OnPrivilegesDenied()
        {
            // close the application
            Tizen.Applications.Application.Current.Exit();
        }
        private void StartMeasurement()
        {
            if (TemperatureSensor.IsSupported)
            {
                _tSensor.DataUpdated += OnT_SensorDataUpdated;
                _tSensor.Start();
                _measuringTemperature = true;
            }
                _monitor.DataUpdated += OnMonitorDataUpdated;
                _monitor.Start();             
                _measuring = true;
            
        }

        private void StopMeasurement()
        {

            _monitor.DataUpdated -= OnMonitorDataUpdated;
            _monitor.Stop();
            _measuring = false;
            if (TemperatureSensor.IsSupported)
            {
                _tSensor.DataUpdated -= OnT_SensorDataUpdated;
                _tSensor.Stop();
                _measuringTemperature = false;
            }
        }
        private void OnMonitorDataUpdated(object sender, HeartRateMonitorDataUpdatedEventArgs e)
        {
            // update displayed value
            heartRateTb.Text = e.HeartRate > 0 ? e.HeartRate.ToString() : "0";
        }
        private void OnT_SensorDataUpdated(object sender, TemperatureSensorDataUpdatedEventArgs e)
        {
            // update displayed value
            TemperatureSensorTb.Text = e.Temperature > 0 ? e.Temperature.ToString() : "0";
        }

        private async void ConnectBrewer_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!BluetoothAdapter.IsBluetoothEnabled)
                {
                    Toast.DisplayText("Please turn on Bluetooth.");
                }
                else
                {
                    ConnectBrewer.IsEnabled = false;
                    BluetoothAdapter.ScanResultChanged += scanResultEventHandler;
                    if (device == null)
                    {
                        BluetoothAdapter.StartLeScan();
                        await WaitScanFlag();
                        BluetoothAdapter.StopLeScan();

                        await Task.Delay(5000);

                    }

                    device = DeviceList.Where(dev => dev.Address.Equals(remote_addr)).FirstOrDefault();
                    if (device != null)
                    {
                        GattClient = device.Ledevice.GattConnect(false);
                        ConnectBrewer.Text = "Found";
                        ConnectBrewer.IsEnabled = false;
                        index_Page.Children[2].IsVisible = true;
                        index_Page.CurrentPage = index_Page.Children[2];
                        //   leDevice.GattConnectionStateChanged += LeDevice_GattConnectionStateChanged;
                        ConnectBrewer.Text = "Connected!";
                    }
                    else
                        ConnectBrewer.IsEnabled = true;

                }
            }
            catch (Exception ex)
            {
                Toast.DisplayText("Error: " + ex.Message);
            }
        }
        public static void LeDevice_GattConnectionStateChanged(object sender, GattConnectionStateChangedEventArgs e)
        {
            if (e.Result != (int)BluetoothError.None)
            {
                StateChanged_flag = false;
            }
            else if (!e.RemoteAddress.Equals(remote_addr))
            {
                StateChanged_flag = false;
            }
            else if (e.IsConnected.Equals(false))
            {
                StateChanged_flag = false;
            }
            else
            {
                StateChanged_flag = true;
            }
        }
        public async Task WaitScanFlag()

        {
            int count = 0;
            do
            {
                await Task.Delay(3000);
                if (count % 4 == 0)
                { 

                }
                count++;

            } while (count < 10 && !scanFlag);

        }
        public static void scanResultEventHandler(object sender, AdapterLeScanResultChangedEventArgs e)
        {
    
            if (!e.DeviceData.Equals(null) )
            {
                Device device = new Device

                {

                    Address = e.DeviceData.RemoteAddress,

                    Ledevice = e.DeviceData

                };
                if (!DeviceList.Any(d => d.Address == device.Address))

                {

                    DeviceList.Add(device);

                }

                scanFlag = true;
            }

        }
        public static void AdvertisingChangedEventHandler(object sender, AdvertisingStateChangedEventArgs e)
        {
            if ((int)e.State == (int)BluetoothLeAdvertisingState.BluetoothLeAdvertisingStarted)
            {
                advertisingFlag = true;
            }
            if ((int)e.State == (int)BluetoothLeAdvertisingState.BluetoothLeAdvertisingStopped)
            {
                advertisingFlag = false;
            }
        }

        private void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            srv_list = GattClient.GetServices();
            selectedService = srv_list.Where(s => s.Uuid.Equals(ServiceUUID)).FirstOrDefault();
            if (selectedService != null)
            {
                charc_list = selectedService.GetCharacteristics();
                selectedcharacteristic = charc_list.Where(c => c.Uuid.Equals(CharacteristicUUID)).FirstOrDefault();
                selectedcharacteristic.SetValue(controlButton.ToString());
                GattClient.WriteValueAsync(selectedcharacteristic);
                Toast.DisplayText("Your tea is brewing!", 3000);
                ConfirmButton.Text = " Brewing ";
            }
            else
                Toast.DisplayText("Bluetooth connection error. Please try again!");
        }
    }
    public class Device
    {
        /// <summary>
        /// Holds the detected remote device object.
        /// </summary>
        public BluetoothLeDevice Ledevice;
        /// <summary>
        /// Contains the Bluetooth address
        /// of the detected remote object.
        /// </summary>
        public string Address { get; set; }
    }
}