using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;


namespace FlightSimulatorApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        public Model model;
        public ViewModel(Model model1)
        {
            this.model = model1;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        //notify the view
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
           
        }

        //change in the view
        public double VM_Throttle
        {
            get { return model.Throttle; }
            set { model.Throttle = value; }
        }

        public double VM_Elevator
        {
            get { return model.Elevator; }
            set { model.Elevator = value; }
        }

        public double VM_Aileron
        {
            get { return model.Aileron; }
            set { model.Aileron = value;}
        }

        public double VM_Rudder
        {
            get { return model.Rudder; }
            set { model.Rudder = value; }
        }

        public double VM_Position_longitude_deg
        {
            get { return model.Position_longitude_deg; }
        }
        public double VM_Position_latitude_deg
        {
            get { return model.Position_latitude_deg; }
        }

        public Location VM_Location
        {
            get { return model.Location; }

        }

        //error
        public string VM_ErrorMessage
        {
            get { return model.ErrorMessage; }
        }

        //exhibit properties
        public string VM_Indicated_heading_deg
        {
            get { 
                if(double.IsInfinity(model.Indicated_heading_deg)){ return "ERROR"; }
                return model.Indicated_heading_deg.ToString();
            }
        }
        public string VM_Gps_indicated_vertical_speed
        {
            get { 
                if (double.IsInfinity(model.Gps_indicated_vertical_speed)) { return "ERROR"; }
                return model.Gps_indicated_vertical_speed.ToString();

            }
        }

        public string VM_Gps_indicated_ground_speed_kt
        {
            get {
                if (double.IsInfinity(model.Gps_indicated_ground_speed_kt)) { return "ERROR"; }
                return model.Gps_indicated_ground_speed_kt.ToString();

            }
        }
        public string VM_Airspeed_indicator_indicated_speed_kt
        {
            get { 
                if (double.IsInfinity(model.Airspeed_indicator_indicated_speed_kt)) { return "ERROR"; }
                return model.Airspeed_indicator_indicated_speed_kt.ToString();

            }
        }
        public string VM_Gps_indicated_altitude_ft
        {
            get { 
                if (double.IsInfinity(model.Gps_indicated_altitude_ft)) { return "ERROR"; }
                return model.Gps_indicated_altitude_ft.ToString();

            }
        }
        public string VM_Attitude_indicator_internal_roll_deg
        {
            get { 
                if (double.IsInfinity(model.Attitude_indicator_internal_roll_deg)) { return "ERROR"; }
                return model.Attitude_indicator_internal_roll_deg.ToString();

            }
        }
        public string VM_Attitude_indicator_internal_pitch_deg
        {
            get { 
                if (double.IsInfinity(model.Attitude_indicator_internal_pitch_deg)) { return "ERROR"; }
                return model.Attitude_indicator_internal_pitch_deg.ToString();

            }

        }
        public string VM_Altimeter_indicated_altitude_ft
        {
            get { 
                if (double.IsInfinity(model.Altimeter_indicated_altitude_ft)) { return "ERROR"; }
                return model.Altimeter_indicated_altitude_ft.ToString();

            }

        }

    }
}
