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

namespace EventDrivenSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Ball test = new Ball();
            test.draw(this);


        }

        public void BouncingBalls()
        {
            int n = 10;
            Ball[] balls = new Ball[n];
            for (int i = 0; i < n; i++)
                balls[i] = new Ball();
            while(true)
            {
                MainLayout.Children.Clear();
                for(int i =0;i<n;i++)
                {
                    balls[i].move(0.5);
                    balls[i].draw(this);
                }

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
            if ((rx + vx * dt < radius) || (rx + vx * dt > 1.0 - radius)) { vx = -vx; }
            if ((ry + vy * dt < radius) || (ry + vy * dt > 1.0 - radius)) { vy = -vy; }
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
            elp.Width = 20;
            elp.Height = 20;
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
