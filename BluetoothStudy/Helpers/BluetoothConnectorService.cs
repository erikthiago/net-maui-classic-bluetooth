#if ANDROID
using Android.Bluetooth;
using Java.Util;
using System.Text;
#endif

namespace BluetoothStudy.Helpers
{
    public class BluetoothConnectorService
    {
        /// <inheritdoc />
        public List<string> GetConnectedDevices()
        {
            // Como fazer a configuração pra acessar o codigo nativo: https://stackoverflow.com/questions/74236294/maui-dependencyservice-getimyservice-return-null/74236424#74236424
            // https://github.com/dotnet/maui-samples/blob/main/8.0/PlatformIntegration/InvokePlatformCodeDemos/InvokePlatformCodeDemos/Services/ConditionalCompilation/DeviceOrientationService.cs
#if ANDROID
            _adapter = GetBluetoothAdapter();

            if (_adapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (_adapter.IsEnabled)
            {
                if (_adapter.BondedDevices.Count > 0)
                    return _adapter.BondedDevices.Select(d => d.Name).ToList();
            }
            else
                Console.Write("Bluetooth is not enabled on device");

#endif
            return new List<string>();
        }

        /// <inheritdoc />
        public void Connect(string deviceName)
        {
#if ANDROID
            var device = _adapter.BondedDevices.FirstOrDefault(d => d.Name == deviceName);
            // Link usado como base: https://arduino.stackexchange.com/questions/31461/identify-uuid-of-hc-06
            _socket = device.CreateInsecureRfcommSocketToServiceRecord(UUID.FromString(SspUdid));

            _socket.Connect();
#endif
        }

        public void TurnOn()
        {
#if ANDROID
            var buffer = "1";

            // Write data to the device to trigger LED
            // Como converter string em byte[]: https://www.c-sharpcorner.com/article/c-sharp-string-to-byte-array/
            _socket.OutputStream.WriteAsync(Encoding.ASCII.GetBytes(buffer), 0, buffer.Length);
#endif
        }

        public void TurnOff()
        {
#if ANDROID
            var buffer = "2";

            // Write data to the device to trigger LED
            // Como converter string em byte[]: https://www.c-sharpcorner.com/article/c-sharp-string-to-byte-array/
            _socket.OutputStream.WriteAsync(Encoding.ASCII.GetBytes(buffer), 0, buffer.Length);
#endif
        }

        public bool IsConnected(string deviceName)
        {
            bool result = false;
#if ANDROID
            result = _adapter.BondedDevices.Count > 0;
#endif
            return result;
        }

        public void Disconnect()
        {
#if ANDROID
            _socket.OutputStream.Close();
            Thread.Sleep(1000);
            _socket.Close();
            _socket = null;
#endif
            Application.Current.Quit();
        }

#if ANDROID
        /// <summary>
        /// The standard UDID for SSP
        /// </summary>
        private const string SspUdid = "00001101-0000-1000-8000-00805f9b34fb";
        private BluetoothAdapter _adapter;
        private BluetoothSocket _socket;

        // Como fazer a config para tirar o warning de obsoleto: https://stackoverflow.com/questions/76766303/how-to-solve-bluetoothadapter-defaultadapter-is-obsoleted-on-android-31-0
        private BluetoothAdapter GetBluetoothAdapter()
        {
            var bluetoothManager = MauiApplication.Current.GetSystemService("bluetooth") as BluetoothManager;
            return bluetoothManager.Adapter;
        }
#endif
    }
}
