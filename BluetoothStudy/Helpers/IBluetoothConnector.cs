namespace BluetoothStudy.Helpers
{
    public interface IBluetoothConnector
    {
        /// <summary>
        /// Gets a list of all Bluetooth devices connected to the phone
        /// </summary>
        /// <returns></returns>
        List<string> GetConnectedDevices();

        /// <summary>
        /// Connects app to Bluetooth device and writes to device
        /// </summary>
        /// <param name="deviceName"></param>
        void Connect(string deviceName);

        void TurnOn();

        void TurnOff();

        bool IsConnected(string deviceName);

        void Disconnect();
    }
}
