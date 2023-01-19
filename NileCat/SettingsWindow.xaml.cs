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
using System.Windows.Shapes;

namespace NileCat
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            serverTb.Text = Properties.Settings.Default.ServerUrl;
            minuteSlider.Value = Properties.Settings.Default.PopopInterval.Ticks / TimeSpan.TicksPerMinute;
            catPopupBtn.IsChecked = Properties.Settings.Default.PopupCats;
        }

        //apply settings click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ServerUrl = serverTb.Text;
            Properties.Settings.Default.PopupCats = catPopupBtn.IsChecked == true;
            Properties.Settings.Default.PopopInterval = new TimeSpan(TimeSpan.TicksPerMinute * (int)minuteSlider.Value);
            Properties.Settings.Default.Save();
            Close();
        }
    }
}
