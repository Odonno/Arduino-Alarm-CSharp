﻿using System;
using System.IO.Ports;
using System.Threading;

namespace ArduinoAlarm.Model
{
    public class Arduino
    {
        #region Fields

        private Timer _detectTimer;
        private readonly object _lock = new object();

        #endregion


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

        /// <summary>
        /// Command 1
        /// </summary>
        public void SendDate()
        {
            lock (_lock)
            {
                SerialPort.Open();

                // Send command byte
                SerialPort.Write(new[] { (byte)1 }, 0, 1);

                // Send hours, minutes, seconds and milliseconds
                SerialPort.WriteLine(DateTime.Now.Hour.ToString());
                SerialPort.WriteLine(DateTime.Now.Minute.ToString());
                SerialPort.WriteLine(DateTime.Now.Second.ToString());
                SerialPort.WriteLine(DateTime.Now.Millisecond.ToString());

                SerialPort.Close();
            }
        }

        /// <summary>
        /// Command 2
        /// </summary>
        /// <param name="on"></param>
        /// <param name="mdp"></param>
        public void SwitchBuzzer(bool on = false, string mdp = null)
        {
            lock (_lock)
            {
                SerialPort.Open();

                byte onByte = on ? (byte)1 : (byte)0;

                // Send command byte + boolean byte
                SerialPort.Write(new[] { (byte)2, onByte }, 0, 2);

                if (!on)
                    SerialPort.WriteLine(mdp);

                SerialPort.Close();
            }
        }

        /// <summary>
        /// Command 3
        /// </summary>
        /// <param name="debut"></param>
        /// <param name="fin"></param>
        public void PlanAlarm(DateTime debut, DateTime fin)
        {
            lock (_lock)
            {
                SerialPort.Open();

                SerialPort.Write(new[] { (byte)3 }, 0, 1);

                SerialPort.WriteLine(debut.Hour.ToString());
                SerialPort.WriteLine(debut.Minute.ToString());
                SerialPort.WriteLine(debut.Second.ToString());
                SerialPort.WriteLine(debut.Millisecond.ToString());

                SerialPort.WriteLine(fin.Hour.ToString());
                SerialPort.WriteLine(fin.Minute.ToString());
                SerialPort.WriteLine(fin.Second.ToString());
                SerialPort.WriteLine(fin.Millisecond.ToString());


                SerialPort.Close();
            }
        }

        /// <summary>
        /// Command 4
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password)
        {
            lock (_lock)
            {
                SerialPort.Open();

                // Send command byte
                SerialPort.Write(new[] { (byte)4 }, 0, 1);

                // Send password
                SerialPort.WriteLine(password);

                SerialPort.Close();
            }
        }

        /// <summary>
        /// Command 5
        /// </summary>
        /// <returns></returns>
        public string AreYouAlive()
        {
            string retour;

            lock (_lock)
            {
                try
                {

                    SerialPort.Open();

                    SerialPort.Write(new[] { (byte)5 }, 0, 1);

                    retour = SerialPort.ReadLine();
                }
                catch
                {
                    retour = "I am not alive";
                }
                finally
                {
                    SerialPort.Close();
                }
            }
            return retour;
        }

        /// <summary>
        /// Command 6
        /// </summary>
        /// <param name="seconds"></param>
        public void DelaiAlarm(int seconds)
        {
            lock (_lock)
            {
                SerialPort.Open();

                SerialPort.Write(new[] { (byte)6 }, 0, 1);

                SerialPort.WriteLine(seconds.ToString());

                SerialPort.Close();
            }
        }

        /// <summary>
        /// Command 7
        /// </summary>
        /// <param name="on"></param>
        public void SwitchLeds(bool on = false)
        {
            lock (_lock)
            {
                SerialPort.Open();

                byte onByte = on ? (byte)1 : (byte)0;

                // Send command byte + boolean byte
                SerialPort.Write(new[] { (byte)7, onByte }, 0, 2);

                SerialPort.Close();
            }
        }

        public void Detect()
        {
            _detectTimer = new Timer(async delegate
            {
                _detectTimer.Change(Timeout.Infinite, Timeout.Infinite);
                Mail mail = null;

                lock (_lock)
                {
                    SerialPort.Open();
                    SerialPort.ReadTimeout = 100;

                    try
                    {
                        string line = SerialPort.ReadLine();

                        // Check if any intrusion is send, and send a mail
                        if (line == "INT")
                        {
                            mail = new Mail
                            {
                                From = "bottiau.david@laposte.net",
                                To = "david.bottiau@epsi.fr",
                                Subject = "Intrusion detected !",
                                Body = "Intrusion detected !"
                            };
                        }

                        // Check if any SOS is send, and send a mail
                        else if (line == "SOS")
                        {
                            mail = new Mail
                            {
                                From = "bottiau.david@laposte.net",
                                To = "david.bottiau@epsi.fr",
                                Subject = "SOS !",
                                Body = "SOS !"
                            };
                        }
                    }
                    catch
                    {
                        mail = null;
                    }
                    finally
                    {
                        SerialPort.Close();
                    }
                }

                if (mail != null)
                    await mail.Send();

                _detectTimer.Change(0, 1000);

            }, null, 0, 1000);
        }

        #endregion
    }
}
