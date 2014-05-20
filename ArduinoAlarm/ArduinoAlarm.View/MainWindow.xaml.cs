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

        private void btActivateBuzzer_Click(object sender, RoutedEventArgs e)
        {
            Arduino.SwitchBuzzer(true);
        }

        private void btDeactiveBuzzer_Click(object sender, RoutedEventArgs e)
        {
            Arduino.SwitchBuzzer();
        }

        private void btSetPassword_Click(object sender, RoutedEventArgs e)
        {
            Arduino.SetPassword(tbPassword.Text);
        }

        private void btPlanAlarm_Click(object sender, RoutedEventArgs e)
        {
            Arduino.PlanAlarm(DateTime.Parse(tbDateDebut.Text), DateTime.Parse(tbDateFin.Text));
        }

        private void btAYA_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Arduino.AreYouAlive());
        }

        private void btDelai_Click(object sender, RoutedEventArgs e)
        {
            Arduino.DelaiAlarm(int.Parse(tbDelai.Text));
        }

        private void btSOS_Click(object sender, RoutedEventArgs e)
        {
            var sosMail = new Mail
            {
                From = "bottiau.david@laposte.net",
                To = "david.bottiau@epsi.fr",
                Subject = "SOS !",
                Body = "SOS !",
            };

            sosMail.Send();
        }
    }
}
