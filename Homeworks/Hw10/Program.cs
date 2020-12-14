using System;
using BenchmarkDotNet.Running;

namespace Hw10
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}