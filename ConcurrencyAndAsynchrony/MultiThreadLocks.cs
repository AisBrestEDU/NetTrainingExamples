using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyAndAsynchrony
{
    class MultiThreadLocks
    {
        private static int _counter = 0;
        private static Random _random = new Random(DateTime.Now.Millisecond);
        private static object _printLock = new Object();
        private static object _counterLock = new Object();

        public static void main()
        {
            lock (_printLock)
            {
                Console.WriteLine("start");
            }
            CreateFuzz();
            var threads = new List<Thread>();
            for (var i = 0; i < 10; i++)
            {

                var thread = new Thread(Process);
                thread.Start();
                threads.Add(thread);
                CreateFuzz();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            lock (_printLock)
            {
                Console.WriteLine("end");
            }
            CreateFuzz();
        }

        private static void Process()
        {
            lock (_counterLock)
            {
                CreateFuzz();
                var oldCnt = _counter;
                CreateFuzz();
                _counter = oldCnt + 1;
                CreateFuzz();

                lock (_printLock)
                {
                    Console.WriteLine("The counter is {0}", _counter);
                    CreateFuzz();
                    Console.WriteLine("--------------------------");
                }
            }

        }

        private static void CreateFuzz()
        {
            Thread.Sleep(_random.Next(10));
        }
    }
}
