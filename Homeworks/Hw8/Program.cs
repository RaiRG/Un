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
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<ICalculator, Calculator>();
            services.AddScoped<IExpressionTree, ExpressionTree>();
            services.AddScoped<ITaskCreator, TaskCreatorWithRequest>();
            services.AddScoped<IGetCalculatedAnswer, ExpressionTreeVisitor>();
            
            var provider = services.BuildServiceProvider();
            var calculator = provider.GetService<ICalculator>();
            
            Console.WriteLine("Введите выражение:");
            var input = Console.ReadLine();
            var result = calculator.CalculateExpression(input);
            Console.WriteLine($"Ответ: {result}");
            Console.ReadKey();
        }
    }
}