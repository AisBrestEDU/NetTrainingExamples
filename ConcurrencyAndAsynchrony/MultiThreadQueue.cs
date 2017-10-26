using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ConcurrencyAndAsynchrony
{
    class MultiThreadQueue
    {
        private static int _counter;
        private static Random _random = new Random(DateTime.Now.Millisecond);

        private static ActionBlock<int> _counterBlock = new ActionBlock<int>(x => CounterManager(x));
        private static ActionBlock<string[]> _printBlock = new ActionBlock<string[]>(x => PrintManager(x));

        public static void main()
        {
            _printBlock.Post(new[] { "start" });
            CreateFuzz();

            var workingThreads = new List<Thread>();
            for (var i = 0; i < 10; i++)
            {
                var thread = new Thread(Process);
                thread.Start();
                workingThreads.Add(thread);
                CreateFuzz();
            }

            foreach (var workingThread in workingThreads)
            {
                workingThread.Join();
            }

            _counterBlock.Complete();
            _counterBlock.Completion.Wait();
            CreateFuzz();
            _printBlock.Post(new[] { "end" });
            _printBlock.Complete();
            _printBlock.Completion.Wait();
        }

        private static void Process()
        {
            CreateFuzz();
            _counterBlock.Post(1);
            CreateFuzz();
        }

        private static void CreateFuzz()
        {
            Thread.Sleep(_random.Next(100));
        }

        private static void CounterManager(int increment)
        {

            CreateFuzz();
            var oldCounter = _counter;
            CreateFuzz();
            _counter = oldCounter + increment;
            CreateFuzz();
            _printBlock.Post(new[]
            {
                    $"The counter is {_counter}",
                    "--------------------------"
                });
            CreateFuzz();
        }

        private static void PrintManager(string[] lines)
        {
            CreateFuzz();
            foreach (var line in lines)
            {
                CreateFuzz();
                Console.WriteLine(line);
                CreateFuzz();
            }
        }
    }
}
