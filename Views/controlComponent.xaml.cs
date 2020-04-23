using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for component.xaml
    /// </summary>
    public partial class controlComponent : UserControl
    {
        public controlComponent()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            DataContext = (Application.Current as App).Vm;
        }

    }
}
