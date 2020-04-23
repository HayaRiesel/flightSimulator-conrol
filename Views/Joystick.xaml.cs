using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlightSimulatorApp.Views
{

    public partial class Joystick : UserControl
    {
        public static readonly DependencyProperty RudderProperty =
            DependencyProperty.Register("Rudder", typeof(double), typeof(Joystick), null);

        public static readonly DependencyProperty ElevatorProperty =
            DependencyProperty.Register("Elevator", typeof(double), typeof(Joystick), null);


        //x
        public double Rudder
        {
            get { return Convert.ToDouble(GetValue(RudderProperty)); }
            set { SetValue(RudderProperty, value); }
        }

        //y
        public double Elevator
        {
            get
            {
                return Convert.ToDouble(GetValue(ElevatorProperty));
            }
            set { SetValue(ElevatorProperty, value); }
        }



        private Point startPosition;
        private double width;
        private double height;
        private readonly System.Windows.Media.Animation.Storyboard centerKnob;
        private bool mouseDown;
        public Joystick()
        {
            InitializeComponent();
            Rudder = 0;
            Elevator = 0;

            mouseDown = false;

            Knob.MouseLeftButtonDown += Knob_MouseDown;
            Knob.MouseLeftButtonUp += Knob_MouseUp;
            Knob.MouseMove += Knob_MouseMove;
            centerKnob = Knob.Resources["CenterKnob"] as System.Windows.Media.Animation.Storyboard;
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rudder = 0;
            Elevator = 0;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mouseDown = true;
                startPosition = e.GetPosition(Base);
                centerKnob.Stop();

                Knob.CaptureMouse();
                width = Base.ActualWidth - KnobBase.ActualWidth;
                height = Base.ActualHeight - KnobBase.ActualHeight;

            }
        }

        // normelize joystick values
        private double NormelizeX(double currrentPosition)
        {
            double num = currrentPosition / width;
            return 2 * num;
        }

        private double NormelizeY(double currrentPosition)
        {
            double num = -currrentPosition / height;
            return 2 * num;
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Point p = new Point(e.GetPosition(Base).X - startPosition.X, e.GetPosition(this).Y - startPosition.Y);
                double distance = Math.Sqrt(p.X * p.X + p.Y * p.Y);

                double radius = width / 2;

                if (distance <= radius)
                {
                    //change the location
                    knobPosition.X = p.X;
                    knobPosition.Y = p.Y;
                    //change the value
                    Rudder = NormelizeX(p.X);
                    Elevator = NormelizeY(p.Y);
                }
                else
                {
                    //make the joystick not to stuck at the edge
                    double x = p.X * radius / distance;
                    double y = p.Y * radius / distance;
                    knobPosition.X = x;
                    knobPosition.Y = y;
                    //change the value
                    Rudder = NormelizeX(x);
                    Elevator = NormelizeY(y);


                }
            }
        }


        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //start the anomation
            Knob.ReleaseMouseCapture();
            centerKnob.Begin();
            mouseDown = false;

        }

        private void CenterKnob_Completed(object sender, EventArgs e)
        {
            Rudder = 0;
            Elevator = 0;

        }

    }
}