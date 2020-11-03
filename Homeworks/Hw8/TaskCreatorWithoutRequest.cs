using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Hw7;

namespace Hw8
{
    public class TaskCreatorWithoutRequest : ITaskCreator
    {
        private string path = "https://localhost:5001/?expression=";

        public async Task<double> CreateTask(Expression expr, Dictionary<Expression, Task<double>> BeforeTasks)
        {
            return await Task.Run(() =>
            {
                if (expr is ConstantExpression expression)
                {
                    return (double) expression.Value;
                }

                var currentBinaryExpression = (BinaryExpression) expr;
                // Получаем Task с двух веток. 
                var left = BeforeTasks[currentBinaryExpression.Left];
                var right = BeforeTasks[currentBinaryExpression.Right];

                // Запускаются обе ветки.
                Task.WhenAll(left, right);
                var operat = GetOperatorForConsolePrint(expr);
                
                var leftOperand = left.Result;
                var rightOperand = right.Result;
                
                var calculator = new Hw7.Calculator();
                var result = calculator.Calculate(operat, leftOperand, rightOperand);
               
                if (result != "Error")
                {
                    Console.WriteLine(left.Result + GetOperatorForConsolePrint(expr) + right.Result + "=" +
                                      result.ToString());
                    return double.Parse(result);
                }

                Console.WriteLine("Ошибка сервера");
                throw new Exception("Invalid data format!");
            });
        }

        private string changheOperat(string operat)
        {
            switch (operat)
            {
                case "+":
                    return "-";
                case "-":
                    return "+";
                default:
                    return operat;
            }
        }

        private bool isPositiveNumber(double number)
        {
            return number >= 0;
        }
        private string GetOperatorForConsolePrint(Expression currentBinaryExpression)
        {
            return currentBinaryExpression.NodeType switch
            {
                ExpressionType.Add => "+",
                ExpressionType.Subtract => "-",
                ExpressionType.Multiply => "*",
                ExpressionType.Divide => "/",
                _ => "+"
            };
        }
    }
}

