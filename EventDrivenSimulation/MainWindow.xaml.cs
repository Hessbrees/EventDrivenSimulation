using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
        Particle[] particles = new Particle[n];
        int time = 0;
        Random rd = new Random();
        MinPQ<string> pq = new MinPQ<string>();
        public MainWindow()
        {
            InitializeComponent();
            //BouncingBalls(0);

            Timer.Tick += new EventHandler(TimeClick);
            Timer.Interval = new TimeSpan(1000);
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
                    double vx = (rd.NextDouble() - rd.NextDouble()) * 0.5;
                    double vy = (rd.NextDouble() - rd.NextDouble()) * 0.5;
                    particles[i] = new Particle(rx, ry, vx, vy);
                }
            }
            CollisionSystem system = new CollisionSystem(particles);
            system.simulate(10000,this);
            
        }

    }


}
