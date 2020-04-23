using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;



namespace FlightSimulatorApp
{

    public class Model : INotifyPropertyChanged
    {
        private TelnetClient telnetClient;
        private volatile Boolean stop;
        private volatile HashSet<string> updateProperty;
        private Dictionary<string, string> simulatorPath;


        public Model(TelnetClient telnetClient1)
        {
            this.telnetClient = telnetClient1;
            this.stop = false;
            updateProperty = new HashSet<string>();
            simulatorPath = SetDictionary();
            this.Location = new Location(32.002644, 34.888781);
            this.errorMessage = "";
        }
        public Dictionary<string, string> SetDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Position_latitude_deg", "/position/latitude-deg");
            dictionary.Add("Position_longitude_deg", "/position/longitude-deg");
            dictionary.Add("Airspeed_indicator_indicated_speed_kt", "/instrumentation/airspeed-indicator/indicated-speed-kt");
            dictionary.Add("Gps_indicated_altitude_ft", "/instrumentation/gps/indicated-altitude-ft");
            dictionary.Add("Attitude_indicator_internal_roll_deg", "/instrumentation/attitude-indicator/internal-roll-deg");
            dictionary.Add("Attitude_indicator_internal_pitch_deg", "/instrumentation/attitude-indicator/internal-pitch-deg");
            dictionary.Add("Altimeter_indicated_altitude_ft", "/instrumentation/altimeter/indicated-altitude-ft");
            dictionary.Add("Indicated_heading_deg", "/instrumentation/heading-indicator/indicated-heading-deg");
            dictionary.Add("Gps_indicated_ground_speed_kt", "/instrumentation/gps/indicated-ground-speed-kt");
            dictionary.Add("Gps_indicated_vertical_speed", "/instrumentation/gps/indicated-vertical-speed");
            return dictionary;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
            if (propertyName == "Throttle" || propertyName == "Rudder" || propertyName == "Elevator" || propertyName == "Aileron")
            {
                updateProperty.Add(propertyName);
            }

        }

        //connetion to the simulator
        public void Connect(string ip, int port)
        {
            this.telnetClient.Connect(ip, port);
        }
        public void Disconnect()
        {
            this.telnetClient.Disconnect();
        }

        //close the thread
        public void Close()
        {
            stop = true;
        }
        //start the thread
        public void Start()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    //update the values by send a set to the server
                    if (updateProperty.Count() != 0)
                    {
                        foreach (string s in updateProperty)
                        {
                            switch (s)
                            {
                                case "Throttle":
                                    telnetClient.Write("set /controls/engines/current-engine/throttle " + throttle.ToString());
                                    try
                                    {
                                        throttle = Convert.ToDouble(telnetClient.Read());
                                    }
                                    catch
                                    {
                                        throttle = double.PositiveInfinity;
                                    }

                                    break;

                                case "Rudder":
                                    telnetClient.Write("set /controls/flight/rudder " + rudder.ToString());
                                    try
                                    {
                                        rudder = Convert.ToDouble(telnetClient.Read());
                                    }
                                    catch
                                    {
                                        rudder = double.PositiveInfinity;
                                    }

                                    break;

                                case "Elevator":
                                    telnetClient.Write("set /controls/flight/elevator " + elevator.ToString());
                                    try
                                    {
                                        elevator = Convert.ToDouble(telnetClient.Read());
                                    }
                                    catch
                                    {
                                        elevator = double.PositiveInfinity;
                                    }

                                    break;

                                case "Aileron":
                                    telnetClient.Write("set /controls/flight/aileron " + aileron.ToString());
                                    try
                                    {
                                        aileron = Convert.ToDouble(telnetClient.Read());
                                    }
                                    catch
                                    {
                                        aileron = double.PositiveInfinity;
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                        //reset the updateProperty
                        updateProperty.Clear();
                    }

                    //get the values from the simulator and update them
                    foreach (KeyValuePair<string, string> kvp in simulatorPath)
                    {
                        telnetClient.Write("get " + kvp.Value);

                        Type myType = this.GetType();
                        PropertyInfo pinfo = myType.GetProperty(kvp.Key);

                        string answer = telnetClient.Read();
                        try
                        {
                            pinfo.SetValue(this, Convert.ToDouble(answer), null);
                            ErrorMessage = "";
                        }
                        //if the answer from the server is not a number
                        catch
                        {
                            if (answer == "ERR\n" || answer == "ERR" || answer == "ERR\n\r")
                            {
                                pinfo.SetValue(this, double.PositiveInfinity, null);
                            }
                            else
                            {
                                ErrorMessage = answer;
                            }

                        }
                    }
                    Thread.Sleep(250);
                }
            }
            ).Start();

        }


        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (!this.errorMessage.Equals(value))
                {
                    this.errorMessage = value;
                    this.NotifyPropertyChanged("ErrorMessage");
                }
            }

        }

        private double elevator;
        public double Elevator
        {
            get { return this.elevator; }
            set
            {
                if (this.elevator != value)
                {
                    this.elevator = value;
                    this.NotifyPropertyChanged("Elevator");
                }
            }
        }

        private double aileron;
        public double Aileron
        {
            get { return this.aileron; }
            set
            {
                if (this.aileron != value)
                {
                    this.aileron = value;
                    this.NotifyPropertyChanged("Aileron");
                }
            }
        }

        private double rudder;
        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                if (this.rudder != value)
                {
                    this.rudder = value;
                    this.NotifyPropertyChanged("Rudder");
                }
            }
        }


        private double throttle;
        public double Throttle
        {
            get { return this.throttle; }
            set
            {
                if (this.throttle != value)
                {
                    this.throttle = value;
                    this.NotifyPropertyChanged("Throttle");
                }
            }
        }

        double indicated_heading_deg;
        public double Indicated_heading_deg
        {
            get { return this.indicated_heading_deg; }
            set
            {
                if (this.indicated_heading_deg != value)
                {
                    this.indicated_heading_deg = value;
                    this.NotifyPropertyChanged("Indicated_heading_deg");
                }
            }
        }
        private double gps_indicated_vertical_speed;
        public double Gps_indicated_vertical_speed
        {
            get { return this.gps_indicated_vertical_speed; }
            set
            {
                if (this.gps_indicated_vertical_speed != value)
                {
                    this.gps_indicated_vertical_speed = value;
                    this.NotifyPropertyChanged("Gps_indicated_vertical_speed");
                }
            }
        }

        private double gps_indicated_ground_speed_kt;
        public double Gps_indicated_ground_speed_kt
        {
            get { return this.gps_indicated_ground_speed_kt; }
            set
            {
                if (this.gps_indicated_ground_speed_kt != value)
                {
                    this.gps_indicated_ground_speed_kt = value;
                    this.NotifyPropertyChanged("Gps_indicated_ground_speed_kt");
                }
            }


        }

        private double airspeed_indicator_indicated_speed_kt;
        public double Airspeed_indicator_indicated_speed_kt
        {
            get { return this.airspeed_indicator_indicated_speed_kt; }
            set
            {
                if (this.airspeed_indicator_indicated_speed_kt != value)
                {
                    this.airspeed_indicator_indicated_speed_kt = value;
                    this.NotifyPropertyChanged("Airspeed_indicator_indicated_speed_kt");
                }
            }


        }

        private double gps_indicated_altitude_ft;
        public double Gps_indicated_altitude_ft
        {
            get { return this.gps_indicated_altitude_ft; }
            set
            {
                if (this.gps_indicated_altitude_ft != value)
                {
                    this.gps_indicated_altitude_ft = value;
                    this.NotifyPropertyChanged("Gps_indicated_altitude_ft");
                }
            }


        }

        private double attitude_indicator_internal_roll_deg;
        public double Attitude_indicator_internal_roll_deg
        {
            get { return this.attitude_indicator_internal_roll_deg; }
            set
            {
                if (this.attitude_indicator_internal_roll_deg != value)
                {
                    this.attitude_indicator_internal_roll_deg = value;
                    this.NotifyPropertyChanged("Attitude_indicator_internal_roll_deg");
                }
            }


        }

        private double attitude_indicator_internal_pitch_deg;
        public double Attitude_indicator_internal_pitch_deg
        {
            get { return this.attitude_indicator_internal_pitch_deg; }
            set
            {
                if (this.attitude_indicator_internal_pitch_deg != value)
                {
                    this.attitude_indicator_internal_pitch_deg = value;
                    this.NotifyPropertyChanged("Attitude_indicator_internal_pitch_deg");
                }
            }


        }

        private double altimeter_indicated_altitude_ft;
        public double Altimeter_indicated_altitude_ft
        {
            get { return this.altimeter_indicated_altitude_ft; }
            set
            {
                if (this.altimeter_indicated_altitude_ft != value)
                {
                    this.altimeter_indicated_altitude_ft = value;
                    this.NotifyPropertyChanged("Altimeter_indicated_altitude_ft");
                }
            }


        }

        private double position_longitude_deg; 
        public double Position_longitude_deg
        {
            get { return this.position_longitude_deg; }
            set
            {
                if (this.position_longitude_deg != value)
                {

                    if (value > 85)
                    {
                        this.position_longitude_deg = 85;
                        ErrorMessage = "ERROR: The plane is out of earth bounds";
                    }
                    else if (value < -85)
                    {
                        this.position_longitude_deg = -85;
                        ErrorMessage = "ERROR: The plane is out of earth bounds";
                    }
                    else { this.position_longitude_deg = value; }

                    this.Location = new Location(position_latitude_deg, position_longitude_deg);
                    this.NotifyPropertyChanged("Position_longitude_deg");
                }
            }


        }

        private double position_latitude_deg;
        public double Position_latitude_deg
        {
            get { return this.position_latitude_deg; }
            set
            {
                if (this.position_latitude_deg != value)
                {
                    if (value > 180)
                    {
                        this.position_latitude_deg = 180;
                        ErrorMessage = "ERROR: The plane is out of earth bounds";
                    }
                    else if (value < -180)
                    {
                        this.position_latitude_deg = -180;
                        ErrorMessage = "ERROR: The plane is out of earth bounds";
                    }
                    else { this.position_latitude_deg = value; }
                    this.Location = new Location(position_latitude_deg, position_longitude_deg);
                    this.NotifyPropertyChanged("Position_latitude_deg");
                }
            }
        }
        private Location location;
        public Location Location
        {
            get { return this.location; }
            set
            {
                if (this.location != value)
                {
                    this.location = value;
                    this.NotifyPropertyChanged("Location");
                }

            }


        }
    }
}

