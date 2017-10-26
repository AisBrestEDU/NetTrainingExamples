using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyAndAsynchrony
{
    class SingleThread
    {
        private static int _counter = 0;

        public static void main()
        {
            for (var i = 0; i < 10; i++)
            {
                Process();
            }
        }

        private static void Process()
        {
            _counter += 1;
            Console.WriteLine($"The counter is {_counter}");
            Console.WriteLine("--------------------------");
        }
    }
}
