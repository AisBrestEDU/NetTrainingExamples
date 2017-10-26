using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyAndAsynchrony
{
    class MultiThreadWithFuzz
    {
        private static int _counter = 0;
        private static Random _random = new Random(DateTime.Now.Millisecond);

        public static void main()
        {
            Console.WriteLine("start");
            CreateFuzz();
            for (var i = 0; i < 10; i++)
            {
                new Thread(Process).Start();
                CreateFuzz();
            }
            Console.WriteLine("end");
            CreateFuzz();
        }

        private static void Process()
        {
            CreateFuzz();
            var oldCnt = _counter;
            CreateFuzz();
            _counter = oldCnt + 1;
            CreateFuzz();
            Console.WriteLine("The counter is {0}", _counter);
            CreateFuzz();
            Console.WriteLine("--------------------------");
        }

        private static void CreateFuzz()
        {
            Thread.Sleep(_random.Next(10));
        }
    }
}
