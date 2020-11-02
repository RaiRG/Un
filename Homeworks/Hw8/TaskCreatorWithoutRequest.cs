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

                var operand1 = left.Result.ToString();
                var operand2 = right.Result.ToString();
                
                var calculator = new Hw7.Calculator();
                var result = calculator.CalculateExpression(operand1 + " " + operat + " " + operand2);
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

