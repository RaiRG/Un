using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace Hw6
{
    class Client
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение:");
            var input = Console.ReadLine();
            var result = Calculator.CalculateExpression(input);
            Console.WriteLine($"Ответ: {result}");
            Console.ReadKey();
        }
    }
}