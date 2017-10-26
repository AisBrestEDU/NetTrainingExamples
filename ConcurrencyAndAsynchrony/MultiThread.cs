using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyAndAsynchrony
{
    public static class MultiThread
    {
        private static int _counter = 0;

        public static void main()
        {
            Console.WriteLine("start");
            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(Process);
                thread.Start();
            }
            Console.WriteLine("end");
        }

        private static void Process()
        {
            _counter += 1;
            Console.WriteLine("The counter is {0}", _counter);
            Console.WriteLine("--------------------------");
        }
    }
}
