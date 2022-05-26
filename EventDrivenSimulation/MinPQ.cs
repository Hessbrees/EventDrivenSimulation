using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace EventDrivenSimulation
{
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
    }

}
