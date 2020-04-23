using System.Windows;

namespace FlightSimulatorApp
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).Vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //make zoom on the map
            myMap.SetView((Application.Current as App).Vm.VM_Location, 13);
        }
    }
}
