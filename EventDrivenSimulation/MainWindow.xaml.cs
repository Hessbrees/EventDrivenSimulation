using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        Particle[] balls = new Particle[n];
        int time = 0;
        Random rd = new Random();

        public MainWindow()
        {
            InitializeComponent();

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
                    balls[i] = new Particle(rx, ry, vx, vy);
                }
            }

            MainLayout.Children.Clear();
            for (int i = 0; i < n; i++)
            {
                balls[i].move(1);
                balls[i].draw(this);
            }
        }

    }

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
        /*        private void redraw(double limit)
                {
                    StdDraw.clear();
                    for (int i = 0; i < particles.length; i++)
                    {
                        particles[i].draw();
                    }
                    StdDraw.show();
                    StdDraw.pause(20);
                    if (t < limit)
                    {
                        pq.insert(new Event(t + 1.0 / HZ, null, null));
                    }
                }*/

        public void simulate(double limit)
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
                //else if (a == null && b == null) redraw(limit);               // redraw event

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
                return this.CompareTo(that);
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

    //MinPQ

    public class MinPQ<Key> where Key :IComparable<Key>
    {
        private Key[] pq;                    // store items at indices 1 to n
        private int n;                       // number of items on priority queue
        public Comparer<Key> comparator;  // optional comparator

        /**
         * Initializes an empty priority queue with the given initial capacity.
         *
         * @param  initCapacity the initial capacity of this priority queue
         */
        public MinPQ(int initCapacity)
        {
            pq = (Key[])new Key[initCapacity + 1];
            n = 0;
        }

        /**
         * Initializes an empty priority queue.
         */
        public MinPQ() : this(1) { }

        public MinPQ(int initCapacity, Comparer<Key> comparator)
        {
            this.comparator = comparator;
            pq = (Key[])new Key[initCapacity + 1];
            n = 0;
        }

        public MinPQ(Comparer<Key> comparator) : this(1, comparator) { }

        public MinPQ(Key[] keys)
        {
            n = keys.Length;
            pq = (Key[])new Key[keys.Length + 1];
            for (int i = 0; i < n; i++)
                pq[i + 1] = keys[i];
            for (int k = n / 2; k >= 1; k--)
                sink(k);
            Debug.Assert(isMinHeap());
        }

        public bool isEmpty()
        {
            return n == 0;
        }

        public int size()
        {
            return n;
        }

        public Key min()
        {
            if (isEmpty()) throw new Exception("Priority queue underflow");
            return pq[1];
        }

        // resize the underlying array to have the given capacity
        private void resize(int capacity)
        {
            Debug.Assert(capacity > n);
            Key[] temp = (Key[])new Key[capacity];
            for (int i = 1; i <= n; i++)
            {
                temp[i] = pq[i];
            }
            pq = temp;
        }

        public void insert(Key x)
        {
            // double size of array if necessary
            if (n == pq.Length - 1) resize(2 * pq.Length);

            // add x, and percolate it up to maintain heap invariant
            pq[++n] = x;
            swim(n);
            Debug.Assert(isMinHeap());
        }
        public Key delMin()
        {
            if (isEmpty()) throw new Exception("Priority queue underflow");
            Key min = pq[1];
            exch(1, n--);
            sink(1);
            pq[n + 1] = default(Key);     // to avoid loitering and help with garbage collection
            if ((n > 0) && (n == (pq.Length - 1) / 4)) resize(pq.Length / 2);
            Debug.Assert(isMinHeap());
            return min;
        }

        private void swim(int k)
        {
            while (k > 1 && greater(k / 2, k))
            {
                exch(k / 2, k);
                k = k / 2;
            }
        }

        private void sink(int k)
        {
            while (2 * k <= n)
            {
                int j = 2 * k;
                if (j < n && greater(j, j + 1)) j++;
                if (!greater(k, j)) break;
                exch(k, j);
                k = j;
            }
        }

        private bool greater(int i, int j)
        {
            if (comparator == null)
            {
                return pq[i].CompareTo(pq[j]) > 0;
            }
            else
            {
                return comparator.Compare(pq[i], pq[j]) > 0;
            }
        }

        private void exch(int i, int j)
        {
            Key swap = pq[i];
            pq[i] = pq[j];
            pq[j] = swap;
        }

        // is pq[1..n] a min heap?
        private bool isMinHeap()
        {
            for (int i = 1; i <= n; i++)
            {
                if (pq[i] == null) return false;
            }
            for (int i = n + 1; i < pq.Length; i++)
            {
                if (pq[i] != null) return false;
            }
            if (pq[0] != null) return false;
            return isMinHeapOrdered(1);
        }

        // is subtree of pq[1..n] rooted at k a min heap?
        private bool isMinHeapOrdered(int k)
        {
            if (k > n) return true;
            int left = 2 * k;
            int right = 2 * k + 1;
            if (left <= n && greater(k, left)) return false;
            if (right <= n && greater(k, right)) return false;
            return isMinHeapOrdered(left) && isMinHeapOrdered(right);
        }

        public IEnumerator<Key> iterator()
        {
            return new HeapIterator();
        }

        public int CompareTo(Key other)
        {
            if (other == null) return 1;
            return this.CompareTo(other);
        }

        private class HeapIterator : IEnumerator<Key>
        {
            public Key Current => throw new NotImplementedException();

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

        /*    public static void main(String[] args)
            {
                MinPQ<String> pq = new MinPQ<String>();
                while (!StdIn.isEmpty())
                {
                    String item = StdIn.readString();
                    if (!item.equals("-")) pq.insert(item);
                    else if (!pq.isEmpty()) StdOut.print(pq.delMin() + " ");
                }
                StdOut.println("(" + pq.size() + " left on pq)");
            }*/

    }


}
