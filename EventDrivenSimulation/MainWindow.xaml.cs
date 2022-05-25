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
        static int n = 1;
        Ball[] balls = new Ball[n];
        int time = 0;
        public MainWindow()
        {
            InitializeComponent();

            Timer.Tick += new EventHandler(TimeClick);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void TimeClick(object sender, EventArgs e)
        {
            BouncingBalls(time);
            time++;
        }
        public void BouncingBalls(int time)
        {

            if(time == 0)
            {
                for (int i = 0; i < n; i++)
                    balls[i] = new Ball();
            }

            MainLayout.Children.Clear();
            for (int i = 0; i < n; i++)
            {
                balls[i].move(5);
                balls[i].draw(this);
            }
        }

    }
    public class Ball
    {
        private double rx, ry;
        private double vx, vy;
        private readonly double radius;
        public Ball()
        {
            // rx/ry 0-480
            rx = 0;
            ry = 0;
            vx = 15;
            vy = 15;
            radius = 10;
        }

        public void move(double dt)
        {
            //if ((rx + vx * dt < radius) || (rx + vx * dt > 480 - radius)) { vx = -vx; }
            //if ((ry + vy * dt < radius) || (ry + vy * dt > 480 - radius)) { vy = -vy; }
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
