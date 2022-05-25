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
using System.Windows.Threading;

namespace EventDrivenSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer = new DispatcherTimer();
        static int n = 10;
        Ball[] balls = new Ball[n];
        int time = 0;
        Random rd = new Random();

        public MainWindow()
        {
            InitializeComponent();

            Timer.Tick += new EventHandler(TimeClick);
            Timer.Interval = new TimeSpan(100000);
            Timer.Start();
        }

        private void TimeClick(object sender, EventArgs e)
        {
            BouncingBalls(time);
            time++;
        }
        public void BouncingBalls(int time)
        {
            if (time == 0)
            {
                for (int i = 0; i < n; i++)
                {
                    double rx = rd.NextDouble() * 480;
                    double ry = rd.NextDouble() * 480;
                    balls[i] = new Ball(rx,ry);
                }    
            }

            MainLayout.Children.Clear();
            for (int i = 0; i < n; i++)
            {
                balls[i].move(0.5);
                balls[i].draw(this);
            }
        }

    }
    public class Ball
    {
        private double rx, ry;
        private double vx, vy;
        private readonly double radius;
        public Ball(double _rx, double _ry)
        {
            rx = _rx;
            ry = _ry;
            vx = 5;
            vy = 5;
            radius = 10;
        }

        public void move(double dt)
        {
            // change radius to 0
            if ((rx + vx * dt < 0) || (rx + vx * dt > 500 - radius*2)) { vx = -vx; }
            if ((ry + vy * dt < 0) || (ry + vy * dt > 500 - radius*2)) { vy = -vy; }
            rx = rx + vx * dt;
            ry = ry + vy * dt;
        }
        public void draw(MainWindow main)
        {
            Ellipse elp = new Ellipse();
            SolidColorBrush BlueBrush = new SolidColorBrush();
            SolidColorBrush BlackBrush = new SolidColorBrush();
            elp.HorizontalAlignment = HorizontalAlignment.Left;
            elp.VerticalAlignment = VerticalAlignment.Top;
            elp.Width = radius*2;
            elp.Height = radius*2;
            BlueBrush.Color = Colors.Blue;
            BlackBrush.Color = Colors.Black;
            elp.Stroke = BlackBrush;
            elp.StrokeThickness = 1;
            elp.Fill = BlueBrush;
            elp.Margin = new Thickness(rx, ry, 0, 0);
            main.MainLayout.Children.Add(elp);

        }

    }
}
