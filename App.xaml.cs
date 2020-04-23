using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        public ViewModel Vm { get; internal set; }

        public Model m;


        private void Application_Startup(Object sender, StartupEventArgs e)
        {
            m = new Model(new TelnetClient());
            Vm = new ViewModel(m);

            MainWindow wnd = new MainWindow();
            wnd.Title = "main window";
            // Show the window
            wnd.Show();
            try
            {
                //m.connect("127.0.0.1", 5402) by the app config
                m.Connect(ConfigurationManager.AppSettings["ip"], int.Parse(ConfigurationManager.AppSettings["port"]));
            }
            catch
            {
                m.ErrorMessage = "you need connect to the serer by the connect window";
            }
            m.Start();



        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //close the threads before the exit
            (Application.Current as App).Vm.model.Disconnect();
            (Application.Current as App).Vm.model.Close();
            Application.Current.Shutdown(99);
        }


    }

}
