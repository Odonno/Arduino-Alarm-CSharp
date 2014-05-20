using System;
using System.IO.Ports;

namespace ArduinoAlarm.Model
{
    public class Arduino
    {
        #region Properties

        public SerialPort SerialPort { get; private set; }
        public const string COMport = "COM5";
        public const int BaudRate = 9600;

        #endregion


        #region Contructor

        public Arduino()
        {
            SerialPort = new SerialPort(COMport, BaudRate);
        }

        #endregion


        #region Methods
        
        public void SendDate()
        {
            SerialPort.Open();

            // Send command byte
            SerialPort.Write(new[] { (byte)1 }, 0, 1);

            // Send hour, minutes, seconds and milliseconds
            SerialPort.WriteLine(DateTime.Now.Hour.ToString());
            SerialPort.WriteLine(DateTime.Now.Minute.ToString());
            SerialPort.WriteLine(DateTime.Now.Second.ToString());
            SerialPort.WriteLine(DateTime.Now.Millisecond.ToString());

            SerialPort.Close();
        }

        public void SwitchBuzzer(bool on = false)
        {
            SerialPort.Open();

            byte onByte = on ? (byte)1 : (byte)0;

            // Send command byte + boolean byte
            SerialPort.Write(new[] { (byte)2, onByte }, 0, 2);

            SerialPort.Close();
        }

        public void SwitchLeds(bool on = false)
        {
            SerialPort.Open();

            byte onByte = on ? (byte)1 : (byte)0;

            // Send command byte + boolean byte
            SerialPort.Write(new[] { (byte)7, onByte }, 0, 2);

            SerialPort.Close();
        }

        #endregion
    }
}
