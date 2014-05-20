using System;
using System.IO.Ports;

namespace ArduinoAlarm.Model
{
    public class Arduino
    {
        #region Properties

        public SerialPort SerialPort { get; private set; }
        public const string COMport = "COM4";
        public const int BaudRate = 9600;

        #endregion


        #region Contructor

        public Arduino()
        {
            SerialPort = new SerialPort(COMport, BaudRate);
        }

        #endregion


        #region Methods

        /// <summary>
        /// command 1
        /// </summary>
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

        /// <summary>
        /// Command 2
        /// </summary>
        /// <param name="on"></param>
        public void SwitchBuzzer(bool on = false)
        {
            SerialPort.Open();

            byte onByte = on ? (byte)1 : (byte)0;

            // Send command byte + boolean byte
            SerialPort.Write(new[] { (byte)2, onByte }, 0, 2);

            SerialPort.Close();
        }

        /// <summary>
        /// Command 3
        /// </summary>
        /// <param name="Debut"></param>
        /// <param name="Fin"></param>
        public void PlanAlarm(DateTime Debut, DateTime Fin)
        {
            SerialPort.Open();

            SerialPort.Write(new[] { (byte)3 }, 0, 1);

            SerialPort.WriteLine(Debut.Hour.ToString());
            SerialPort.WriteLine(Debut.Minute.ToString());
            SerialPort.WriteLine(Debut.Second.ToString());
            SerialPort.WriteLine(Debut.Millisecond.ToString());

            SerialPort.WriteLine(Fin.Hour.ToString());
            SerialPort.WriteLine(Fin.Minute.ToString());
            SerialPort.WriteLine(Fin.Second.ToString());
            SerialPort.WriteLine(Fin.Millisecond.ToString());


            SerialPort.Close();

        }

        /// <summary>
        /// command 4
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password)
        {
            SerialPort.Open();

            // Send command byte
            SerialPort.Write(new[] { (byte)4 }, 0, 1);

            // Send password
            SerialPort.WriteLine(password);

            SerialPort.Close();
        }
        /// <summary>
        /// command 5
        /// </summary>
        /// <returns></returns>
        public string AYA()
        {
            string retour;
            try
            {
                SerialPort.Open();

                SerialPort.Write(new[] { (byte)5 }, 0, 1);

                retour = SerialPort.ReadLine();
            }
            catch (Exception e)
            {
                retour = "I am not alive";
            }
            finally
            {
                SerialPort.Close();
            }

            return retour;
        }

        /// <summary>
        /// command 6
        /// </summary>
        /// <param name="sec"></param>
        public void delaiAlarm(int sec)
        {
            SerialPort.Open();

            SerialPort.Write(new[] { (byte) 6}, 0, 1);

            SerialPort.WriteLine(sec.ToString());

            SerialPort.Close();
        }

        /// <summary>
        /// command 7
        /// </summary>
        /// <param name="on"></param>
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
