using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Jaide.SystemConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PerformanceCounter[] counters = new PerformanceCounter[System.Environment.ProcessorCount];
            for (int i = 0; i < counters.Length; i++)
            {
                counters[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
            }

            while (true)
            {
                for (int i = 0; i < counters.Length; i++)
                {
                    float f = counters[i].NextValue();
                    System.Console.WriteLine("CPU-{0}: {1:f}%", i, f);
                }
                

                System.Console.WriteLine();
                System.Threading.Thread.Sleep(1000);
            }

        }
    }
}
