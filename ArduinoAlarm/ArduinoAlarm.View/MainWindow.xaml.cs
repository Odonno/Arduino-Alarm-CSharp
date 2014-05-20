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
        public Arduino Arduino { get { return _arduino; } }


        public MainWindow()
        {
            InitializeComponent();

            Arduino.SendDate();
        }


        private void btAllumerLed_Click(object sender, RoutedEventArgs e)
        {
            Arduino.SwitchLeds(true);
        }

        private void btEteindreLed_Click(object sender, RoutedEventArgs e)
        {
            Arduino.SwitchLeds();
        }
    }
}
