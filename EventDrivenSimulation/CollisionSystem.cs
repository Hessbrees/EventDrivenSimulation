using System;
using System.Windows;

namespace EventDrivenSimulation
{
    public class CollisionSystem
    {
        private static readonly double HZ = 0.5;    // number of redraw events per clock tick

        private MinPQ<Event> pq;          // the priority queue
        private double t = 0.0;          // simulation clock time
        private Particle[] particles;     // the array of particles

        public CollisionSystem(Particle[] particles)
        {
            this.particles = (Particle[])particles.Clone();  // defensive copy
        }

        // updates priority queue with all new events for particle a
        private void predict(Particle a, double limit)
        {
            if (a == null) return;

            // particle-particle collisions
            for (int i = 0; i < particles.Length; i++)
            {
                double dt = a.timeToHit(particles[i]);
                if (t + dt <= limit)
                    pq.insert(new Event(t + dt, a, particles[i]));
            }

            // particle-wall collisions
            double dtX = a.timeToHitVerticalWall();
            double dtY = a.timeToHitHorizontalWall();
            if (t + dtX <= limit) pq.insert(new Event(t + dtX, a, null));
            if (t + dtY <= limit) pq.insert(new Event(t + dtY, null, a));
        }

        // redraw all particles
        //Change this.
        private void redraw(double limit, MainWindow win)
        {
            win.MainLayout.Children.Clear();
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].draw(win);
            }
            //StdDraw.show();
            //StdDraw.pause(20);
            if (t < limit)
            {
                pq.insert(new Event(t + 1.0 / HZ, null, null));
            }
        }

        public void simulate(double limit,MainWindow win)
        {

            // initialize PQ with collision events and redraw event
            pq = new MinPQ<Event>();
            for (int i = 0; i < particles.Length; i++)
            {
                predict(particles[i], limit);
            }
            pq.insert(new Event(0, null, null));        // redraw event


            // the main event-driven simulation loop
            while (!pq.isEmpty())
            {

                // get impending event, discard if invalidated
                Event e = pq.delMin();
                if (!e.isValid()) continue;
                Particle a = e.a;
                Particle b = e.b;

                // physical collision, so update positions, and then simulation clock
                for (int i = 0; i < particles.Length; i++)
                    particles[i].move(e.time - t);
                t = e.time;

                // process event
                if (a != null && b != null) a.bounceOff(b);              // particle-particle collision
                else if (a != null && b == null) a.bounceOffVerticalWall();   // particle-wall collision
                else if (a == null && b != null) b.bounceOffHorizontalWall(); // particle-wall collision
                else if (a == null && b == null) redraw(limit,win);               // redraw event

                // update the priority queue with new collisions involving a or b
                predict(a, limit);
                predict(b, limit);
            }
        }

        private class Event : IComparable<Event>
        {
            public double time;         // time that event is scheduled to occur
            public Particle a, b;       // particles involved in event, possibly null
            private readonly int countA, countB;  // collision counts at event creation


            // create a new event to occur at time t involving a and b
            public Event(double t, Particle a, Particle b)
            {
                this.time = t;
                this.a = a;
                this.b = b;
                if (a != null) countA = a.Count();
                else countA = -1;
                if (b != null) countB = b.Count();
                else countB = -1;
            }

            // compare times when two events will occur
            public int CompareTo(Event that)
            {
                return this.time.CompareTo(that.time);
            }

            // has any collision occurred between when event was created and now?
            public bool isValid()
            {
                if (a != null && a.Count() != countA) return false;
                if (b != null && b.Count() != countB) return false;
                return true;
            }
        }

    }


}
