using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EventDrivenSimulation
{
    public class Particle
    {
        private double rx, ry;
        private double vx, vy;
        private readonly double radius;
        private readonly double mass;
        private int count;
        public Particle(double _rx, double _ry,double _vx, double _vy)
        {
            rx = _rx;
            ry = _ry;
            vx = _vx;
            vy = _vy;
            radius = 10;
            mass = 0.5;
        }

        public void move(double dt)
        {
            // change radius to 0
            //if ((rx + vx * dt < 0) || (rx + vx * dt > 500 - radius*2)) { vx = -vx; }
            //if ((ry + vy * dt < 0) || (ry + vy * dt > 500 - radius*2)) { vy = -vy; }
            rx = rx + vx;
            ry = ry + vy;
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
        #region TimeToHit
        public double timeToHit(Particle that)
        {
            if (this == that) return double.PositiveInfinity;
            double dx = that.rx - this.rx;
            double dy = that.ry - this.ry;
            double dvx = that.vx - this.vx;
            double dvy = that.vy - this.vy;
            double dvdr = dx * dvx + dy * dvy;
            if (dvdr > 0) return double.PositiveInfinity;
            double dvdv = dvx * dvx + dvy * dvy;
            if (dvdv == 0) return double.PositiveInfinity;
            double drdr = dx * dx + dy * dy;
            double sigma = this.radius + that.radius;
            double d = (dvdr * dvdr) - dvdv * (drdr - sigma * sigma);
            // if (drdr < sigma*sigma) StdOut.println("overlapping particles");
            if (d < 0) return double.PositiveInfinity;
            return -(dvdr + Math.Sqrt(d)) / dvdv;
        }
        public double timeToHitVerticalWall()
        {
            if (vx > 0) return (500 - ry - radius * 2) / vy;
            else if (vx < 0) return (radius*2 - rx) / vx;
            else return double.PositiveInfinity;
        }

        public double timeToHitHorizontalWall()
        {
            if (vy > 0) return (500 - ry - radius*2) / vy;
            else if (vy < 0) return (radius*2 - ry) / vy;
            else return double.PositiveInfinity;
        }
        #endregion

        #region bounceOff
        public void bounceOff(Particle that)
        {
            double dx = that.rx - this.rx;
            double dy = that.ry - this.ry;
            double dvx = that.vx - this.vx;
            double dvy = that.vy - this.vy;
            double dvdr = dx * dvx + dy * dvy;             // dv dot dr
            double dist = this.radius + that.radius;   // distance between particle centers at collison

            // magnitude of normal force
            double magnitude = 2 * this.mass * that.mass * dvdr / ((this.mass + that.mass) * dist);

            // normal force, and in x and y directions
            double fx = magnitude * dx / dist;
            double fy = magnitude * dy / dist;

            // update velocities according to normal force
            this.vx += fx / this.mass;
            this.vy += fy / this.mass;
            that.vx -= fx / that.mass;
            that.vy -= fy / that.mass;

            // update collision counts
            this.count++;
            that.count++;
        }

        public void bounceOffVerticalWall()
        {
            vx = -vx;
            count++;
        }
        public void bounceOffHorizontalWall()
        {
            vy = -vy;
            count++;
        }
        #endregion
        public double kineticEnergy()
        {
            return 0.5 * mass * (vx * vx + vy * vy);
        }
        public int Count()
        {
            return count;
        }
    }
}
