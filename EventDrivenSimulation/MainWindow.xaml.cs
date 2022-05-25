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
            test.draw();
        }




    }
    public class Ball
    {
        private double rx, ry;
        private double vx, vy;
        private readonly double radius;
        public Ball()
        {
            rx = 10;
            ry = 10;
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
        public void draw()
        {
            MainWindow win = new MainWindow();
            Ellipse elp = new Ellipse();
            SolidColorBrush BlueBrush = new SolidColorBrush();
            SolidColorBrush BlackBrush = new SolidColorBrush();
            BlueBrush.Color = Colors.Blue;
            BlackBrush.Color = Colors.Black;
            elp.Fill = BlueBrush;
            elp.Stroke = BlackBrush;
            elp.Width = 10;
            elp.Height = 10;
            win.MainLayout.Children.Add(elp);

        }

    }
}
