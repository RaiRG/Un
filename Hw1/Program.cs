﻿using System;
using static Hw1.Calculator;
using static Hw1.UserInteraction;

namespace Hw1
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = GetNumber();
            var @operator = Console.ReadLine();
            var b = GetNumber();
            var result = Calculate(@operator, a, b);
            Console.WriteLine(result);

        }
    }
}
