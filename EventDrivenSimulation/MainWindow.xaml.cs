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
        }
    }

    public class BouncingBalls
    {
        public static void main(String[] args)
        {
            int n = Int32.Parse(args[0]);
            for (int i = 0; i < n; i++)
                ;
            while (true)
            {

            }
        }
    }

    public class Ball
    {
        private double rx, ry;
        private double vx, vy;
        private double radius;
        public Ball() { }

        public void move(double dt)
        {
            if((rx+vx*dt<radius)||(rx+vx*dt>1.0 - radius)) { vx = -vx; }
            if((ry+vy*dt<radius)||(ry+vy*dt>1.0 - radius)) { vy = -vy; }
            rx = rx + vx * dt;
            ry = ry + vy * dt;

        }
        public void draw()
        {
            //stddraw
        }


    }

}
