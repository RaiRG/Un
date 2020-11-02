using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

using Microsoft.Extensions.DependencyInjection;

namespace Hw8
{
    class Client
    {
        static void Main(string[] args)
        {
            IServiceCollection sc = new ServiceCollection(); 
            sc.AddScoped<ICalculator, Calculator>();
            
            var sp = sc.BuildServiceProvider();
            Console.WriteLine("Введите выражение:");
            var input = Console.ReadLine();
            var result = Calculator.CalculateExpression(input);
            Console.WriteLine($"Ответ: {result}");
            Console.ReadKey();
        }
    }
}