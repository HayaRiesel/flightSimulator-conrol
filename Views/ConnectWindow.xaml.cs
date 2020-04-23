using System.Configuration;
using System.Windows;
using System.Windows.Controls;
namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for connectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : UserControl
    {
        public ConnectWindow()
        {
            InitializeComponent();

        }

        private void HideErrorMassage()
        {
            error.Visibility = Visibility.Hidden;
            errorMessage.Visibility = Visibility.Hidden;
        }
        private void ShowErrorMassage()
        {
            error.Visibility = Visibility.Visible;
            errorMessage.Visibility = Visibility.Visible;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //disconnect before the new connection
            (Application.Current as App).m.Disconnect();
            try
            {
                (Application.Current as App).m.Connect(this.ip.Text, int.Parse(this.port.Text));
            }
            catch
            {
                ShowErrorMassage();

            }
            ip.Text = "";
            port.Text = "";
        }

        private void Error_Click(object sender, RoutedEventArgs e)
        {
            HideErrorMassage();
        }

        private void ErrorConnectDefault_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).m.Disconnect();
            try
            {
                (Application.Current as App).m.Connect(ConfigurationManager.AppSettings["ip"], int.Parse(ConfigurationManager.AppSettings["port"]));
            }
            catch
            {
                ShowErrorMassage();
            }
        }
    }
}
