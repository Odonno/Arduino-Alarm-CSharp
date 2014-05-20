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
    }
}
