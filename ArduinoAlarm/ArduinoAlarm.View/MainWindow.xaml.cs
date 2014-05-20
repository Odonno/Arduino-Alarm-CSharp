using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ArduinoAlarm.Model;

namespace ArduinoAlarm.View
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Arduino _arduino = new Arduino();


        public MainWindow()
        {
            InitializeComponent();
        }


        private void btAllumerLed_Click(object sender, RoutedEventArgs e)
        {
            SwitchLeds(true);
        }

        private void btEteindreLed_Click(object sender, RoutedEventArgs e)
        {
            SwitchLeds();
        }

        private void SwitchLeds(bool on = false)
        {
            _arduino.SerialPort.Open();
            byte onByte = on ? (byte)1 : (byte)0;

            for (byte i = 5; i < 8; i++)
                _arduino.SerialPort.Write(new [] { i, onByte }, 0, 2);

            _arduino.SerialPort.Close();
        }
    }
}
