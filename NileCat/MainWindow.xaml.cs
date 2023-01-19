using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using NileCat.WindowPlacement;

namespace NileCat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string serverUrl;

        private string errorText;

        private Storyboard errorSb;

        public string ErrorText { get { return errorText; } set { errorLbl.BeginStoryboard(errorSb); errorText = value; OnPropertyChanged(); } }

        private bool popupCats;

        private bool inSettings;

        private DispatcherTimer timer;

        private BitmapImage catImage;

        public BitmapImage CatImage { get { return catImage; } set { catImage = value;OnPropertyChanged(); } }

        HttpClient http;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string ServerUrl { get { return serverUrl; } }

        public MainWindow()
        {
            serverUrl = Properties.Settings.Default.ServerUrl;
            http = new HttpClient();

            timer = new DispatcherTimer();

            timer.Interval = Properties.Settings.Default.PopopInterval;
            timer.Tick += Timer_Tick;

            popupCats = Properties.Settings.Default.PopupCats;

            timer.Start();

            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(popupCats)
            {
                Button_Click_1(null, null);
            }
        }

        //settings btn click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new SettingsWindow();

            inSettings = true;
            wnd.ShowDialog();
            inSettings = false;
            timer.Interval = Properties.Settings.Default.PopopInterval;
            serverUrl = Properties.Settings.Default.ServerUrl;
            popupCats = Properties.Settings.Default.PopupCats;
            OnPropertyChanged(nameof(serverUrl));
        }

        //shwo cat click
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Delay(2000);
                var imgUrl = await http.GetStringAsync(Properties.Settings.Default.ServerUrl + "/api/cat-img-v1");

                CatImage = new BitmapImage(new Uri(imgUrl, UriKind.Absolute));

                CatImage.DownloadCompleted += CatImage_DownloadCompleted;
                CatImage.DecodeFailed += CatImage_GetFailed;
                CatImage.DownloadFailed += CatImage_GetFailed;

                //"https://i.ytimg.com/vi/W_BRoGB6nto/hqdefault.jpg"
                ErrorText = "";
            }
            catch(Exception ez)
            {
                if (e != null)
                    MessageBox.Show("Could not get the cat. Reason: " + ez.Message, "NileCat", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    ErrorText = "Failed to get a cat. Reason: " + ez.Message;
                    if (!inSettings)
                    {
                        FocusWindow();
                    }
                }
            }
        }

        private void FocusWindow()
        {
            if (!this.IsVisible)
            {
                this.Show();
            }

            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }

            this.Topmost = true;  // important
            this.Topmost = false; // important
            this.Focus();         // important
        }

        private void CatImage_DownloadCompleted(object sender, EventArgs e)
        {
            if (!inSettings)
            {
                FocusWindow();
            }
        }

        private void CatImage_GetFailed(object sender, ExceptionEventArgs e)
        {
            ErrorText = "Couldn't get the cat. Reason: " + e.ErrorException.Message;
            if (!inSettings)
            {
                FocusWindow();
            }
        }

        private void mainWnd_Closed(object sender, EventArgs e)
        {
            http.Dispose();
        }

        private void mainWnd_Loaded(object sender, RoutedEventArgs e)
        {
            errorSb = this.FindResource("errorTextAnim") as Storyboard;
            this.SetPlacement(Properties.Settings.Default.PlacementXml);
            Button_Click_1(null, new RoutedEventArgs());
        }

        //help btn click
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("A program to periodically show you random images of cats. Ask The Nile about it for more info.\n" +
                "While the server URL is customisable, do not change it without first consulting The Nile. Happy cats <3",
                "NileCat", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void mainWnd_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.PlacementXml = this.GetPlacement();
            Properties.Settings.Default.Save();
        }
    }
}
